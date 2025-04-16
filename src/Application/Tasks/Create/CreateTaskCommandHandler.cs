using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;

namespace Application.Tasks.Create;

internal sealed class CreateTaskCommandHandler(
    IApplicationDbContext context , IUserContext userContext) : ICommandHandler<CreateTaskCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;
        Board? board = await context.Boards
            .SingleOrDefaultAsync(x => x.Id == request.BoardId && x.CreatedById == userId, cancellationToken: cancellationToken);
        
        if(board is null)
        {
            return Result.Failure<Guid>(BoardErrors.NotFound(request.BoardId));
        }

        var deadLine = DateOnly.FromDateTime(request.DeadLine);

        var task = new Task
        {
            Board = board,
            DeadLine = deadLine,
            Description = request.Description,
            Name = request.Name,
            Order = request.Order,
            Priority = request.Priority,
        };

        context.Tasks.Add(task);
        await context.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}
