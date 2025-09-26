using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.Get;

internal sealed class GetProjectsQueryHandler(
    IApplicationDbContext context, IUserAccess userAccess) : IQueryHandler<GetProjectsQuery, List<ProjectResponse>>
{
    public async Task<Result<List<ProjectResponse>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        bool hasAccess = await userAccess.IsAuthenticatedAsync(request.WorkspaceId);
        if (!hasAccess)
        {
            return Result.Failure<List<ProjectResponse>>(WorkspaceErrors.NotFound(request.WorkspaceId));
        }

        List<ProjectResponse> projects = await context.Projects
            .AsNoTracking()
            .Where(x => x.Workspace.Id == request.WorkspaceId)
            .Select(x => new ProjectResponse(x.Id, x.Name, x.Color)).ToListAsync(cancellationToken);

        return projects;
    }
}
