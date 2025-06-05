using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.Get;

internal sealed class GetBoardsQueryHandler(
    IApplicationDbContext context, IUserContext userContext) : IQueryHandler<GetBoardsQuery, List<BoardResponse>>
{
    public async Task<Result<List<BoardResponse>>> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;

        return await context.Boards
            .AsNoTracking()
            .Where(x => x.Project.Id == request.ProjectId && x.CreatedById == userId)
            .OrderBy(x => x.Order)
            .ThenBy(x=>x.CreatedOnUtc)
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
