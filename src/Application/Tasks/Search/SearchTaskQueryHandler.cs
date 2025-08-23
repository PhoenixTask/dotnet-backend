using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Boards.GetBoardTask;
using Application.Users.Access;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.Search;

internal sealed class SearchTaskByProjectQueryHandler(
    IApplicationDbContext context,
    IUserAccess userAccess
) : IQueryHandler<SearchTaskByProjectQuery, List<BoardResponse>>
{
    public async Task<Result<List<BoardResponse>>> Handle(SearchTaskByProjectQuery request, CancellationToken cancellationToken)
    {
        Project? project = await context.Projects
            .Include(x => x.Workspace)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);
        if (project is null)
        {
            return Result.Failure<List<BoardResponse>>(ProjectErrors.NotFound(request.ProjectId));
        }

        bool hasAccess = await userAccess.IsAuthenticatedAsync(project.Workspace.Id);
        if (!hasAccess)
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
                        DeadLine = t.DeadLine.ToString(),
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
