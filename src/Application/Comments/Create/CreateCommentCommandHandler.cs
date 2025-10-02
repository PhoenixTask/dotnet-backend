using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;

namespace Application.Comments.Create;

internal sealed class CreateCommentCommandHandler
    (IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<CreateCommentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        Task? task = await context.Tasks
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ThenInclude(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);

        if (task is null)
        {
            return Result.Failure<Guid>(TaskErrors.NotFound(request.TaskId));
        }
        bool hasAccess = await userAccess.IsAuthenticatedAsync(task.Board.Project.Workspace.Id);
        if (!hasAccess)
        {
            return Result.Failure<Guid>(TaskErrors.NotFound(request.TaskId));
        }
        var comment = new Comment
        {
            Content = request.Content.Trim(),
            Task = task
        };

        context.Comments.Add(comment);
        await context.SaveChangesAsync(cancellationToken);
        return comment.Id;
    }
}
