using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Task = Domain.Tasks.Task;

namespace Application.Comments.Create;

internal sealed class CreateCommentCommandHandler
    (IApplicationDbContext context) : ICommandHandler<CreateCommentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        Task? task = await context.Tasks.SingleOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);

        if (task is null)
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
