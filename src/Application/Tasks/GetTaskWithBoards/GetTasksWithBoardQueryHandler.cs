using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Tasks.GetTaskByDate;
using Application.Users.Access;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.GetTaskWithBoards;

internal sealed class GetTasksWithBoardQueryHandler(IApplicationDbContext context, IUserAccess userAccess) : IQueryHandler<GetTasksWithBoardQuery, List<TaskResponse>>
{
    public async Task<Result<List<TaskResponse>>> Handle(GetTasksWithBoardQuery request, CancellationToken cancellationToken)
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
               .Include(x => x.Board)
               .Where(x => x.Board.ProjectId == request.ProjectId);
        if (!request.IncludeCompleted)
        {
            taskQuery = taskQuery
                .Where(x => !x.IsComplete);
        }
        return await taskQuery
               .Select(x => new TaskResponse
               {
                   BoardId = x.BoardId,
                   BoardName = x.Board.Name,
                   DeadLine = x.DeadLine.ToString(),
                   Name = x.Name,
                   Id = x.Id,
                   IsComplete = x.IsComplete,
                   Order = x.Order,
                   Priority = x.Priority
               }).ToListAsync(cancellationToken);
    }
}
