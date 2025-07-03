using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Comments.Get;

internal sealed class GetTaskCommentsQueryHandler
    (IApplicationDbContext context) : IQueryHandler<GetTaskCommentsQuery, List<CommentResponse>>
{
    public async Task<Result<List<CommentResponse>>> Handle(GetTaskCommentsQuery request, CancellationToken cancellationToken)
    {
        return await context.Comments.Include(x => x.CreatedBy)
            .Where(x => x.TaskId == request.TaskId)
            .Select(x => new CommentResponse
            {
                CommentId = x.Id,
                Content = x.Content,
                PublishedOn = x.CreatedOnUtc,
                UserName = x.CreatedBy!.UserName,
                FullName = $"{x.CreatedBy.FirstName} {x.CreatedBy.LastName}"
            }).ToListAsync(cancellationToken);
    }
}
