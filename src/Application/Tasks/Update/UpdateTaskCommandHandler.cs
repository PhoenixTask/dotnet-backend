using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Common;
using Domain.Projects;
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

        Board? board = await context.Boards.SingleOrDefaultAsync(x => x.Id == request.BoardId && x.CreatedById == userId, cancellationToken);

        if (board is null)
        {
            return Result.Failure(BoardErrors.NotFound(request.BoardId));
        }

        task.Priority = request.Priority;
        task.Board = board;
        task.DeadLine = DateOnly.FromDateTime(request.DeadLine.GetValueOrDefault());
        task.Description = request.Description;
        task.Name = request.Name;
        task.Priority = request.Priority;

        context.Tasks
          .Where(x => x.BoardId== task.BoardId)
          .PutInOrder(ref task, request.Order);

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
