using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Comments.Delete;

internal sealed class DeleteCommentCommandHandler
    (IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<DeleteCommentCommand>
{
    public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        Comment? comment = await context.Comments
            .Include(x => x.Task)
            .ThenInclude(x => x.Board)
            .ThenInclude(x => x.Project)
            .ThenInclude(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.CommentId, cancellationToken);

        if (comment is null)
        {
            return Result.Failure(CommentErrors.NotFound(request.CommentId));
        }
        bool hasAccess = await userAccess.IsAuthenticatedAsync(comment.Task.Board.Project.Workspace.Id);
        if (hasAccess)
        {
            return Result.Failure(CommentErrors.NotFound(request.CommentId));
        }
        context.Comments.Remove(comment);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
