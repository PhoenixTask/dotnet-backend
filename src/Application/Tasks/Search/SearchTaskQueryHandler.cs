using System.Globalization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Boards.GetBoardTask;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.Search;

internal sealed class SearchTaskByProjectQueryHandler(
    IApplicationDbContext context
) : IQueryHandler<SearchTaskByProjectQuery, List<BoardResponse>>
{
    public async Task<Result<List<BoardResponse>>> Handle(SearchTaskByProjectQuery request, CancellationToken cancellationToken)
    {
        Guid workspaceId = await context.Projects
            .AsNoTracking()
            .Where(x => x.Id == request.ProjectId)
            .Select(x => x.Workspace.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (workspaceId.Equals(Guid.Empty))
        {
            return Result.Failure<List<BoardResponse>>(ProjectErrors.NotFound(request.ProjectId));
        }

        string parameter = request.Parameter?.Trim() ?? string.Empty;
        string likePattern = $"%{parameter}%";

        List<BoardResponse> boards = await context.Boards
            .AsNoTracking()
            .Where(b => b.Project.Id == request.ProjectId)
            .Select(b => new BoardResponse
            {
                Id = b.Id,
                Name = b.Name,
                Color = b.Color,
                IsArchive = b.IsArchive,
                Order = b.Order,
                TaskResponses = b.Tasks
                    .Where(t =>
                        EF.Functions.Like(t.Name, likePattern) ||
                        EF.Functions.Like(t.Description, likePattern))
                    .Select(t => new TaskResponse
                    {
                        Id = t.Id,
                        DeadLine = t.DeadLine.GetValueOrDefault().ToString(new CultureInfo("en-US")),
                        Description = t.Description,
                        Name = t.Name,
                        Order = t.Order,
                        Priority = t.Priority
                    })
                    .ToList()
            })
            .Where(b => b.TaskResponses.Any())
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result.Success(boards);
    }
}
