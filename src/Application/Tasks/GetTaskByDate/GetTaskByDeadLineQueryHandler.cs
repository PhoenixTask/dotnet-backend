using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Projects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.GetTaskByDate;

internal sealed class GetTaskByDeadLineQueryHandler(IApplicationDbContext context, IUserAccess userAccess,IFormatProvider formatProvider) : IQueryHandler<GetTaskByDeadLineQuery, List<TaskResponse>>
{
    public async Task<Result<List<TaskResponse>>> Handle(GetTaskByDeadLineQuery request, CancellationToken cancellationToken)
    {
        Project? project = await context.Projects
            .Include(x => x.Workspace)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);
        if (project is null)
        {
            return Result.Failure<List<TaskResponse>>(ProjectErrors.NotFound(request.ProjectId));
        }

        bool hasAccess = await userAccess.IsAuthenticatedAsync(project.Workspace.Id);
        if (!hasAccess)
        {
            return Result.Failure<List<TaskResponse>>(ProjectErrors.NotFound(request.ProjectId));
        }
        IQueryable<Domain.Tasks.Task> taskQuery = context.Tasks
            .AsNoTracking()
            .Include(x => x.Board)
            .Where(x => x.Board.ProjectId == request.ProjectId)
            .Where(x => x.DeadLine >= DateOnly.FromDateTime(request.StartDate))
            .Where(x => x.DeadLine <= DateOnly.FromDateTime(request.EndDate));

        if (!request.IncludeCompleted)
        {
            taskQuery = taskQuery
                .Where(x => !x.IsComplete);
        }

        return await taskQuery
            .Select(x => new TaskResponse
            {
                DeadLine = x.DeadLine.HasValue ? x.DeadLine.Value.ToString(formatProvider):string.Empty,
                Id = x.Id,
                IsComplete = x.IsComplete,
                Name = x.Name,
                Order = x.Order,
                Priority = x.Priority,
                BoardId = x.Board.Id,
                BoardName = x.Board.Name,
            })
            .ToListAsync(cancellationToken);
    }
}
