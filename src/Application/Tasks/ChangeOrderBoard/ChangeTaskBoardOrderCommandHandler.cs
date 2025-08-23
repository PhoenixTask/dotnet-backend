using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.ChangeOrderBoard;

internal sealed class ChangeTaskBoardOrderCommandHandler
    (IApplicationDbContext context, IUserContext userContext) : ICommandHandler<ChangeTaskBoardOrderCommand>
{
    public async Task<Result> Handle(ChangeTaskBoardOrderCommand request, CancellationToken cancellationToken)
    {
        var taskIds = request.TaskRequests.ToDictionary(r => r.TaskId);

        Guid[] accessibleBoardIds = await context.Members
            .Include(x => x.Workspace)
            .ThenInclude(x => x.Projects)
            .ThenInclude(x => x.Boards)
            .Where(x => x.UserId == userContext.UserId)
            .SelectMany(x => x.Workspace.Projects)
            .SelectMany(x => x.Boards)
            .Select(b => b.Id)
            .Where(x => taskIds.Values.Select(a => a.BoardId).Contains(x))
            .ToArrayAsync(cancellationToken);

        taskIds = taskIds.Where(x => accessibleBoardIds.Contains(x.Value.BoardId)).ToDictionary();

        Task[] tasks = await context.Members
            .Include(x => x.Workspace)
            .ThenInclude(x => x.Projects)
            .ThenInclude(x => x.Boards)
            .ThenInclude(x => x.Tasks)
            .Where(x => x.UserId == userContext.UserId)
            .SelectMany(x => x.Workspace.Projects)
            .SelectMany(x => x.Boards)
            .SelectMany(x => x.Tasks)
            .Where(x => taskIds.Keys.Contains(x.Id))
            .ToArrayAsync(cancellationToken);

        foreach (Task task in tasks)
        {
            task.Order = taskIds[task.Id].Order;
            task.BoardId = taskIds[task.Id].BoardId;
        }

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
