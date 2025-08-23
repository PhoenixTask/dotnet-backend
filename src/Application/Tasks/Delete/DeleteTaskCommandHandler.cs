using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.Delete;

internal sealed class DeleteTaskCommandHandler(
    IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<DeleteTaskCommand>
{
    public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
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
        if (hasAccess)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        context.Tasks.Remove(task);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
