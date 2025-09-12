using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Boards.Get;
using Application.Users.Access;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.GetById;

internal sealed class GetBoardByIdQueryHandler(IApplicationDbContext context, IUserAccess userAccess) : IQueryHandler<GetBoardByIdQuery, BoardResponse>
{
    public async Task<Result<BoardResponse>> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        Guid workspaceId = await context.Boards
              .AsNoTracking()
              .Include(x => x.Project)
              .ThenInclude(x => x.Workspace)
              .Where(x => x.Id == request.BoardId)
              .Select(x => x.Project.Workspace.Id)
        .SingleOrDefaultAsync(cancellationToken);

        bool hasAccess = await userAccess.IsAuthenticatedAsync(workspaceId);
        if (!hasAccess)
        {
            return Result.Failure<BoardResponse>(BoardErrors.NotFound(request.BoardId));
        }

        return await context.Boards
            .AsNoTracking()
            .Select(x => new BoardResponse
            {
                Id = x.Id,
                Color = x.Color,
                Name = x.Name,
                IsArchive = x.IsArchive,
                Order = x.Order,
            })
            .SingleAsync(x => x.Id == request.BoardId, cancellationToken);
    }
}
