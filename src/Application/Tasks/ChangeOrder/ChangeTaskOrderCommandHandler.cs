using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;


namespace Application.Tasks.ChangeOrder;

internal sealed class ChangeTaskOrderCommandHandler
    (IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<ChangeTaskOrderCommand>
{
    public async Task<Result> Handle(ChangeTaskOrderCommand request, CancellationToken cancellationToken)
    {
        Task? task = await context.Tasks
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ThenInclude(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        bool hasAccess = await userAccess.IsAuthenticatedAsync(task.Board.Project.Workspace.Id);
        if (!hasAccess)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        List<Task> tasks = await context.Tasks
            .Where(x => x.BoardId == task.BoardId && x.Id != task.Id)
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        for (int i = 0; i < tasks.Count; i++) // recalculate orders (fill the gaps) 
        {
            tasks[i].Order = i + 1;
        }
        task.Order = request.Order; // Sets task order to user defined order
        foreach (Task t in tasks.Where(x => x.Order >= request.Order)) // Shift all tasks order by one which bigger than current order
        {
            t.Order++;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
