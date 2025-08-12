using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Workspaces.Get;

internal sealed class GetWorkspacesQueryHandler(IApplicationDbContext context, IUserContext userContext)
    : IQueryHandler<GetWorkspacesQuery, List<WorkspaceResponse>>
{
    public async Task<Result<List<WorkspaceResponse>>> Handle(GetWorkspacesQuery request, CancellationToken cancellationToken)
    {
        return await context.Members
            .Include(x => x.Workspace)
            .AsNoTracking()
            .Where(x => x.UserId == userContext.UserId)
            .Select(x => new WorkspaceResponse(x.Workspace.Id, x.Workspace.Name, x.Workspace.Color))
         .ToListAsync(cancellationToken);
    }
}
