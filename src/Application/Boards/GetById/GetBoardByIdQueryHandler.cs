using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Boards.Get;
using Application.Users.AccessAction;
using Domain.Projects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.GetById;

internal sealed class GetBoardByIdQueryHandler(IUserContext userContext, ISender sender, IApplicationDbContext context) : IQueryHandler<GetBoardByIdQuery, BoardResponse>
{
    public async Task<Result<BoardResponse>> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        bool boardExists = await context.Boards
           .AnyAsync(x => x.Id == request.BoardId, cancellationToken);

        if (!boardExists)
        {
            return Result.Failure<BoardResponse>(BoardErrors.NotFound(request.BoardId));
        }

        UserAccessCommand accessRequest = new(userContext.UserId, request.BoardId, typeof(Board));
        Result accessResult = await sender.Send(accessRequest, cancellationToken);
        if (accessResult.IsFailure)
        {
            return Result.Failure<BoardResponse>(accessResult.Error);
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
