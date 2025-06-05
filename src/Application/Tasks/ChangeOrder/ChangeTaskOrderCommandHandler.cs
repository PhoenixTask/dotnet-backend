using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Common;
using Domain.Projects;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;


namespace Application.Tasks.ChangeOrder;

internal sealed class ChangeTaskOrderCommandHandler
    (IApplicationDbContext context, IUserContext userContext) : ICommandHandler<ChangeTaskOrderCommand>
{
    public async Task<Result> Handle(ChangeTaskOrderCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;
        Task? task = await context.Tasks.SingleOrDefaultAsync(x => x.Id == request.TaskId && x.CreatedById == userId, cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        context.Tasks
           .Where(x => x.BoardId == task.BoardId)
           .PutInOrder(ref task, request.Order);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
