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
        Board? board = await context.Boards
            .SingleOrDefaultAsync(x => x.Id == request.BoardId && x.CreatedById == userId, cancellationToken);

        if (board is null)
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
