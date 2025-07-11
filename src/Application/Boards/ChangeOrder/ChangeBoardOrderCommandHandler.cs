using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.ChangeOrder;

internal sealed class ChangeBoardOrderCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<ChangeBoardOrderCommand>
{
    public async Task<Result> Handle(ChangeBoardOrderCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;
        Board? board = await context.Boards.SingleOrDefaultAsync(x => x.Id == request.BoardId && x.CreatedById == userId, cancellationToken);

        if (board is null)
        {
            return Result.Failure(BoardErrors.NotFound(request.BoardId));
        }

        board.Order = request.Order;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
