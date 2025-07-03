using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Comments.Update;

internal sealed class UpdateCommentCommandHandler
    (IApplicationDbContext context) : ICommandHandler<UpdateCommentCommand>
{
    public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        Comment? comment = await context.Comments.SingleOrDefaultAsync(x => x.Id == request.CommentId, cancellationToken);
        if (comment is null)
        {
            return Result.Failure(CommentErrors.NotFound(request.CommentId));
        }

        comment.Content = request.Content.Trim();
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
