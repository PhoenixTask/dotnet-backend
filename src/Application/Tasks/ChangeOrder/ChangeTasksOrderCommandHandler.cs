using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.ChangeOrder;

internal sealed class ChangeTasksOrderCommandHandler
    (IApplicationDbContext context) : ICommandHandler<ChangeTasksOrderCommand>
{
    public async Task<Result> Handle(ChangeTasksOrderCommand request, CancellationToken cancellationToken)
    {
        var taskIds = request.Tasks.Select(x => x.Id).ToList();

        List<Domain.Tasks.Task> tasks = await context.Tasks
            .Where(t => taskIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

        foreach (Domain.Tasks.Task task in tasks)
        {
            int? newOrder = request.Tasks
                .FirstOrDefault(x => x.Id == task.Id)?.Order;

            if (newOrder.HasValue)
            {
                task.Order = newOrder.Value;
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
