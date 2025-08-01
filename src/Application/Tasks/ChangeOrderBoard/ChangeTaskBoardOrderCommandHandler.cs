using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.ChangeOrderBoard;

internal sealed class ChangeTaskBoardOrderCommandHandler
    (IApplicationDbContext context) : ICommandHandler<ChangeTaskBoardOrderCommand>
{
    public async Task<Result> Handle(ChangeTaskBoardOrderCommand request, CancellationToken cancellationToken)
    {
        var taskIds = request.TaskRequests.Select(t => t.TaskId).ToList();

        List<Domain.Tasks.Task> tasks = await context.Tasks
            .Where(t => taskIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

        if (tasks.Count != request.TaskRequests.Count)
        {
            Guid missingId = taskIds.Except(tasks.Select(t => t.Id)).First();
            return Result.Failure(TaskErrors.NotFound(missingId));
        }

        foreach (TaskRequest taskRequest in request.TaskRequests)
        {
            Domain.Tasks.Task task = tasks.First(t => t.Id == taskRequest.TaskId);
            task.BoardId = taskRequest.BoardId;
            task.Order = taskRequest.Order;
        }
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
        throw new NotImplementedException();
    }
}
