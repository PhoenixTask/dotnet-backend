using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.Update;

internal sealed class UpdateBoardCommandHandler(
    IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<UpdateBoardCommand>
{
    public async Task<Result> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        Board? board = await context.Boards
            .Include(x => x.Project)
            .ThenInclude(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.BoardId, cancellationToken);

        if (board is null)
        {
            return Result.Failure(BoardErrors.NotFound(request.BoardId));
        }
        bool hasAccess = await userAccess.IsAuthenticatedAsync(board.Project.Workspace.Id);
        if (!hasAccess)
        {
            return Result.Failure(BoardErrors.NotFound(request.BoardId));
        }

        board.Name = request.Name;
        board.Color = request.Color;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
