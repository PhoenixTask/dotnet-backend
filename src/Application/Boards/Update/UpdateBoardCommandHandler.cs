using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.Update;

internal sealed class UpdateBoardCommandHandler(
    IApplicationDbContext context, IUserContext userContext) : ICommandHandler<UpdateBoardCommand>
{
    public async Task<Result> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;
        Board? board = await context.Boards.SingleOrDefaultAsync(x => x.Id == request.BoardId && x.CreatedById == userId, cancellationToken);

        if (board is null)
        {
            return Result.Failure(BoardErrors.NotFound(request.BoardId));
        }

        board.Name = request.Name;
        board.Color = request.Color;
        board.Order = request.Order;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
