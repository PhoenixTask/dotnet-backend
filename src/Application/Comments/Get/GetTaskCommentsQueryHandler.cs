using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Comments.Get;

internal sealed class GetTaskCommentsQueryHandler
    (IApplicationDbContext context, IUserAccess userAccess) : IQueryHandler<GetTaskCommentsQuery, List<CommentResponse>>
{
    public async Task<Result<List<CommentResponse>>> Handle(GetTaskCommentsQuery request, CancellationToken cancellationToken)
    {
        Guid workspaceId = await context.Tasks
            .AsNoTracking()
            .Include(x => x.Board)
            .ThenInclude(x => x.Project)
            .ThenInclude(x => x.Workspace)
            .Where(x => x.Id == request.TaskId)
            .Select(x => x.Board.Project.Workspace.Id)
            .SingleOrDefaultAsync(cancellationToken);

        bool hasAccess = await userAccess.IsAuthenticatedAsync(workspaceId);
        if (hasAccess)
        {
            return Result.Failure<List<CommentResponse>>(TaskErrors.NotFound(request.TaskId));
        }
        return await context.Comments
            .Include(x => x.CreatedBy)
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
