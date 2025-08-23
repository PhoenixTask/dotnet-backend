using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.Update;

internal sealed class UpdateTaskCommandHandler
    (IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<UpdateTaskCommand>
{
    public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        Task? task = await context.Tasks
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ThenInclude(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.Id));
        }

        bool hasAccess = await userAccess.IsAuthenticatedAsync(task.Board.Project.Workspace.Id);
        if (!hasAccess)
        {
            return Result.Failure(TaskErrors.NotFound(request.Id));
        }

        task.DeadLine = request.DeadLine.HasValue ? DateOnly.FromDateTime(request.DeadLine.Value) : null;
        task.Description = request.Description;
        task.Name = request.Name;
        task.Priority = request.Priority;

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
