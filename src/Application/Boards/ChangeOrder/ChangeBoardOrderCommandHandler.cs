using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Projects;
using Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.ChangeOrder;

internal sealed class ChangeBoardOrderCommandHandler(IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<ChangeBoardOrderCommand>
{
    public async Task<Result> Handle(ChangeBoardOrderCommand request, CancellationToken cancellationToken)
    {
        Board? board = await context.Boards
            .Include(x => x.Project)
            .ThenInclude(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.BoardId, cancellationToken);

        if (board is null)
        {
            return Result.Failure(BoardErrors.NotFound(request.BoardId));
        }

        bool hasAccess = await userAccess.IsAuthenticatedAsync(board.Project.Workspace.Id, Role.Owner);
        if (!hasAccess)
        {
            return Result.Failure(BoardErrors.NotFound(request.BoardId));
        }

        List<Board> boards = await context.Boards
            .Where(x => x.ProjectId == board.ProjectId && x.Id != board.Id)
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        for (int i = 0; i < boards.Count; i++)
        {
            boards[i].Order = i + 1;
        }
        board.Order = request.Order;
        foreach (Board? b in boards.Where(x => x.Order >= request.Order))
        {
            b.Order++;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
