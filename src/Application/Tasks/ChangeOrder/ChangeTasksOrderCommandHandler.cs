using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;
namespace Application.Tasks.ChangeOrder;

internal sealed class ChangeTasksOrderCommandHandler
    (IApplicationDbContext context, IUserContext userContext) : ICommandHandler<ChangeTasksOrderCommand>
{
    public async Task<Result> Handle(ChangeTasksOrderCommand request, CancellationToken cancellationToken)
    {
        var taskIds = request.Tasks.ToDictionary(r => r.Id);

        List<Task> tasks = await context.Members
            .Include(x => x.Workspace)
            .ThenInclude(x => x.Projects)
            .ThenInclude(x => x.Boards)
            .ThenInclude(x => x.Tasks)
            .Where(x => x.UserId == userContext.UserId)
            .SelectMany(x => x.Workspace.Projects)
            .SelectMany(x => x.Boards)
            .SelectMany(x => x.Tasks)
            .Where(x => taskIds.Keys.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (Task task in tasks)
        {
            task.Order = taskIds[task.Id].Order;
        }

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
