using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.ChangeOrder;

internal sealed class ChangeBoardsOrderCommandHandler
    (IApplicationDbContext context) : ICommandHandler<ChangeBoardsOrderCommand>
{
    public async Task<Result> Handle(ChangeBoardsOrderCommand request, CancellationToken cancellationToken)
    {
        var boardIds = request.Boards.Select(x => x.Id).ToList();

        List<Board> boards = await context.Boards
            .Where(b => boardIds.Contains(b.Id))
            .ToListAsync(cancellationToken);

        foreach (Board board in boards)
        {
            int? newOrder = request.Boards
                .FirstOrDefault(x => x.Id == board.Id)?.Order;

            if (newOrder.HasValue)
            {
                board.Order = newOrder.Value;
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
