using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;


namespace Application.Tasks.SwitchBoard;

internal sealed class SwitchBoardCommandHandler
    (IApplicationDbContext context, IUserContext userContext) : ICommandHandler<SwitchBoardCommand>
{
    public async Task<Result> Handle(SwitchBoardCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;
        Task? task = await context.Tasks.SingleOrDefaultAsync(x => x.Id == request.TaskId && x.CreatedById == userId, cancellationToken);

        if (task is null)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        Board? board = await context.Boards.SingleOrDefaultAsync(x => x.Id == request.BoardId && x.CreatedById == userId, cancellationToken);

        if (board is null)
        {
            return Result.Failure(BoardErrors.NotFound(request.BoardId));
        }

        task.Board = board;

        return Result.Success();
    }
}
