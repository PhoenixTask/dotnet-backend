using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Tasks.Delete;

internal sealed class DeleteTaskCommandHandler(
    IApplicationDbContext context, IUserContext userContext) : ICommandHandler<DeleteTaskCommand>
{
    public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;

        int countDeleted = await context.Tasks
            .Where(x => x.Id == request.TaskId && x.CreatedById == userId)
            .ExecuteDeleteAsync(cancellationToken);

        return countDeleted > 0 ? Result.Success() : Result.Failure(TaskErrors.NotFound(request.TaskId));
    }
}
