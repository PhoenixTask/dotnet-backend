using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Projects;
using Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.Delete;

internal sealed class DeleteBoardCommandHandler(
    IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<DeleteBoardCommand>
{
    public async Task<Result> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        Board? board = await context.Boards
            .Include(x => x.Tasks)
            .Include(x => x.Project)
            .ThenInclude(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.boardId, cancellationToken);

        if (board is null)
        {
            return Result.Failure(BoardErrors.NotFound(request.boardId));
        }
        bool hasAccess = await userAccess.IsAuthenticatedAsync(board.Project.Workspace.Id, Role.Owner);
        if (!hasAccess)
        {
            return Result.Failure(BoardErrors.NotFound(request.boardId));
        }

        context.Boards.Remove(board);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
