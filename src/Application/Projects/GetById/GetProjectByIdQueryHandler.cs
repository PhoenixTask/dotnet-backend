using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Projects.Get;
using Application.Users.Access;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.GetById;

internal sealed class GetProjectByIdQueryHandler(IApplicationDbContext context, IUserAccess userAccess) : IQueryHandler<GetProjectByIdQuery, ProjectResponse>
{
    public async Task<Result<ProjectResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        Project? project = await context.Projects
            .AsNoTracking()
            .Include(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

        if (project is null)
        {
            return Result.Failure<ProjectResponse>(ProjectErrors.NotFound(request.ProjectId));
        }

        bool hasAccess = await userAccess.IsAuthenticatedAsync(project.Workspace.Id);
        if (!hasAccess)
        {
            return Result.Failure<ProjectResponse>(ProjectErrors.NotFound(request.ProjectId));
        }
        return new ProjectResponse(project.Id, project.Name, project.Color);
    }
}
