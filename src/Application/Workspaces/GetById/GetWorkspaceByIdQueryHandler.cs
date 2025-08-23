using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Application.Workspaces.Get;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Workspaces.GetById;

internal sealed class GetWorkspaceByIdQueryHandler(IUserAccess userAccess, IApplicationDbContext context) : IQueryHandler<GetWorkspaceByIdQuery, WorkspaceResponse>
{
    public async Task<Result<WorkspaceResponse>> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        WorkspaceResponse? workspace = await context.Members
            .AsNoTracking()
            .Include(x => x.Workspace)
            .Where(x => x.WorkspaceId == request.WorkspaceId)
            .Select(x => new WorkspaceResponse(x.Workspace.Id, x.Workspace.Name, x.Workspace.Color))
            .SingleOrDefaultAsync(cancellationToken);

        bool hasAccess = await userAccess.IsAuthenticatedAsync(request.WorkspaceId);

        if (workspace is null || !hasAccess)
        {
            return Result.Failure<WorkspaceResponse>(WorkspaceErrors.NotFound(request.WorkspaceId));
        }

        return workspace;
    }
}
