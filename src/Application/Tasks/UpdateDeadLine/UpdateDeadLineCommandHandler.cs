using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.UpdateDeadLine;

internal sealed class UpdateDeadLineCommandHandler
    (IApplicationDbContext context) : ICommandHandler<UpdateDeadLineCommand>
{
    public async Task<Result> Handle(UpdateDeadLineCommand request, CancellationToken cancellationToken)
    {
        Domain.Tasks.Task? task = await context.Tasks.SingleOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);
        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        var deadLine = DateOnly.FromDateTime(request.NewDeadLine.GetValueOrDefault());

        task.DeadLine = request.NewDeadLine.HasValue ? deadLine : null;

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
