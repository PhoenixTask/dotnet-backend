using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.Update;

internal sealed class UpdateTaskCommandHandler
    (IApplicationDbContext context, IUserContext userContext) : ICommandHandler<UpdateTaskCommand>
{
    public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;
        Task? task = await context.Tasks.SingleOrDefaultAsync(x => x.Id == request.Id && x.CreatedById == userId, cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.Id));
        }

        task.Priority = request.Priority;
        task.DeadLine = DateOnly.FromDateTime(request.DeadLine.GetValueOrDefault());
        task.Description = request.Description;
        task.Name = request.Name;
        task.Priority = request.Priority;

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
