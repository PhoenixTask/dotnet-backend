using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Tasks.Get;
using Application.Users.Access;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;
namespace Application.Tasks.GetById;

internal sealed class GetTaskByIdQueryHandler(IApplicationDbContext context, IUserAccess userAccess) : IQueryHandler<GetTaskByIdQuery, TaskResponse>
{
    public async Task<Result<TaskResponse>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        Task? task = await context.Tasks
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ThenInclude(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);
        if (task is null)
        {
            return Result.Failure<TaskResponse>(TaskErrors.NotFound(request.TaskId));
        }

        bool hasAccess = await userAccess.IsAuthenticatedAsync(task.Board.Project.Workspace.Id);
        if (!hasAccess)
        {
            return Result.Failure<TaskResponse>(TaskErrors.NotFound(request.TaskId));
        }

        return new TaskResponse
        {
            DeadLine = task.DeadLine.ToString(),
            Description = task.Description,
            Id = request.TaskId,
            IsComplete = task.IsComplete,
            Name = task.Name,
            Order = task.Order,
            Priority = task.Priority
        };
    }
}
