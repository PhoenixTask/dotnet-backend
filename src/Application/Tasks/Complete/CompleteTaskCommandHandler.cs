using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.AccessAction;
using Domain.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.Complete;

internal sealed class CompleteTaskCommandHandler(ISender sender,IUserContext userContext,IApplicationDbContext context) : ICommandHandler<CompleteTaskCommand>
{
    public async Task<Result> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        Result accessResult = await sender.Send(new UserAccessCommand(userContext.UserId, request.TaskId, typeof(Domain.Tasks.Task)), cancellationToken);
        if (accessResult.IsFailure)
        {
            return accessResult;
        }
        Domain.Tasks.Task? task = await context.Tasks.SingleOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);
        if(task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }
        task.IsComplete = request.IsComplete;
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
