using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.Update;

internal sealed class UpdateProjectCommandHandler(
    IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<UpdateProjectCommand, Result>
{
    public async Task<Result<Result>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        Project? project = await context.Projects
            .Include(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

        if (project is null)
        {
            return Result.Failure(ProjectErrors.NotFound(request.ProjectId));
        }

        bool hasAccess = await userAccess.IsAuthenticatedAsync(project.Workspace.Id);
        if (!hasAccess)
        {
            return Result.Failure(ProjectErrors.NotFound(request.ProjectId));
        }

        project.Name = request.Name;
        project.Color = request.Color;
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
