using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.Get;

internal sealed class GetTasksQueryHandler(IApplicationDbContext context, IUserContext userContext) : IQueryHandler<GetTasksQuery, List<TaskResponse>>
{
    public async Task<Result<List<TaskResponse>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        return await context.Members
            .AsNoTracking()
            .Include(x => x.Workspace)
            .ThenInclude(x => x.Projects)
            .ThenInclude(x => x.Boards)
            .ThenInclude(x => x.Tasks)
            .Where(x => x.UserId == userContext.UserId)
            .SelectMany(x => x.Workspace.Projects)
            .SelectMany(x => x.Boards)
            .SelectMany(x => x.Tasks)
            .Where(x => x.BoardId == request.BoardId)
            .Select(x => new TaskResponse
            {
                Id = x.Id,
                DeadLine = x.DeadLine.ToString(),
                Description = x.Description,
                Name = x.Name,
                Order = x.Order,
                Priority = x.Priority,
                IsComplete = x.IsComplete
            }).ToListAsync(cancellationToken);
    }
}
