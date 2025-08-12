using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Boards.Get;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.GetById;

internal sealed class GetBoardByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetBoardByIdQuery, BoardResponse>
{
    public async Task<Result<BoardResponse>> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        bool boardExists = await context.Boards
           .AnyAsync(x => x.Id == request.BoardId, cancellationToken);

        if (!boardExists)
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
