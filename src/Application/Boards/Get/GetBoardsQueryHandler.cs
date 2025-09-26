using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.Get;

internal sealed class GetBoardsQueryHandler(
    IApplicationDbContext context, IUserAccess userAccess) : IQueryHandler<GetBoardsQuery, List<BoardResponse>>
{
    public async Task<Result<List<BoardResponse>>> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
    {
        Guid workspaceId = await context.Projects
            .AsNoTracking()
            .Include(x => x.Workspace)
            .Where(x => x.Id == request.ProjectId)
            .Select(x => x.Workspace.Id)
            .SingleOrDefaultAsync(cancellationToken);

        bool hasAccess = await userAccess.IsAuthenticatedAsync(workspaceId);
        if (!hasAccess)
        {
            return Result.Failure<List<BoardResponse>>(ProjectErrors.NotFound(request.ProjectId));
        }
        return await context.Boards
            .AsNoTracking()
            .Where(x => x.Project.Id == request.ProjectId)
            .OrderBy(x => x.Order)
            .ThenBy(x => x.CreatedOnUtc)
            .Select(x => new BoardResponse
            {
                Id = x.Id,
                Name = x.Name,
                Color = x.Color,
                IsArchive = x.IsArchive,
                Order = x.Order,
            }).ToListAsync(cancellationToken);
    }
}
