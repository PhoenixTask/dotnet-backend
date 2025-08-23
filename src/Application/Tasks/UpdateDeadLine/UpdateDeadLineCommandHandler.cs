using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.UpdateDeadLine;

internal sealed class UpdateDeadLineCommandHandler
    (IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<UpdateDeadLineCommand>
{
    public async Task<Result> Handle(UpdateDeadLineCommand request, CancellationToken cancellationToken)
    {
        Task? task = await context.Tasks
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

        task.DeadLine = request.NewDeadLine.HasValue ? DateOnly.FromDateTime(request.NewDeadLine.Value) : null;

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
