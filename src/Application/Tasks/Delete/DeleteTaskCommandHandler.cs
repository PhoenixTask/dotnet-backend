using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.Delete;

internal sealed class DeleteTaskCommandHandler(
    IApplicationDbContext context, IUserContext userContext) : ICommandHandler<DeleteTaskCommand>
{
    public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;
        Task? task = await context.Tasks
            .SingleOrDefaultAsync(x => x.Id == request.TaskId && x.CreatedById == userId, cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        context.Tasks.Remove(task);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
