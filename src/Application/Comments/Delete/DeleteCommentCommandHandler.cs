using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Comments.Delete;

internal sealed class DeleteCommentCommandHandler
    (IApplicationDbContext context) : ICommandHandler<DeleteCommentCommand>
{
    public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        Comment? comment = await context.Comments.SingleOrDefaultAsync(x => x.Id == request.CommentId, cancellationToken);

        if (comment is null)
        {
            return Result.Failure(CommentErrors.NotFound(request.CommentId));
        }

        context.Comments.Remove(comment);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
