using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Projects;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;


namespace Application.Tasks.SwitchBoard;

internal sealed class SwitchBoardCommandHandler
    (IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<SwitchBoardCommand>
{
    public async Task<Result> Handle(SwitchBoardCommand request, CancellationToken cancellationToken)
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

        bool userAccessTask = await userAccess.IsAuthenticatedAsync(task.Board.Project.Workspace.Id);
        if (!userAccessTask)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        Board? board = await context.Boards
            .Include(x => x.Project)
            .ThenInclude(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.BoardId, cancellationToken);

        if (board is null)
        {
            return Result.Failure(BoardErrors.NotFound(request.BoardId));
        }

        bool userAccessBoard = await userAccess.IsAuthenticatedAsync(board.Project.Workspace.Id);
        if (!userAccessBoard)
        {
            return Result.Failure(TaskErrors.NotFound(request.TaskId));
        }

        task.Board = board;

        return Result.Success();
    }
}
