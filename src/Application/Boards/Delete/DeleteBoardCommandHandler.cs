using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.Delete;

internal sealed class DeleteBoardCommandHandler(
    IApplicationDbContext context, IUserContext userContext) : ICommandHandler<DeleteBoardCommand>
{
    public async Task<Result> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;
        Board? board = await context.Boards
            .SingleOrDefaultAsync(x => x.Id == request.boardId && x.CreatedById == userId, cancellationToken);

        if (board is null)
        {
            return Result.Failure(BoardErrors.NotFound(request.boardId));
        }

        context.Boards.Remove(board);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
