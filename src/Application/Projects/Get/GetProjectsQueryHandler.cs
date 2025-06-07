using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.Get;

internal sealed class GetProjectsQueryHandler(
    IApplicationDbContext context, IUserContext userContext) : IQueryHandler<GetProjectsQuery, List<ProjectResponse>>
{
    public async Task<Result<List<ProjectResponse>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;

        bool projectExist = await context.Workspaces
            .AnyAsync(x => x.Id == request.WorkspaceId && x.CreatedById == userId, cancellationToken);

        if (!projectExist)
        {
            return Result.Failure<List<ProjectResponse>>(WorkspaceErrors.NotFound(request.WorkspaceId));
        }

        List<ProjectResponse> projects = await context.Projects
            .AsNoTracking()
            .Where(x => x.Workspace.Id == request.WorkspaceId && x.CreatedById == userId)
            .Select(x => new ProjectResponse
            {
                Id = x.Id,
                Name = x.Name,
                Color = x.Color
            }).ToListAsync(cancellationToken);

        return projects;
    }
}
