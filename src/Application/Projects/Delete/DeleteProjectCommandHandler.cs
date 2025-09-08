using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.Delete;

internal sealed class DeleteProjectCommandHandler(
    IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<DeleteProjectCommand>
{
    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {

        Project? project = await context.Projects
            .Include(x => x.Workspace)
            .Include(x => x.Boards)
            .ThenInclude(x => x.Tasks)
            .Where(x => x.Id == request.ProjectId)
            .SingleOrDefaultAsync(cancellationToken);

        if (project is null)
        {
            return Result.Failure(ProjectErrors.NotFound(request.ProjectId));
        }

        bool hasAccess = await userAccess.IsAuthenticatedAsync(project.Workspace.Id);
        if (hasAccess)
        {
            return Result.Failure(ProjectErrors.NotFound(request.ProjectId));
        }

        context.Projects.Remove(project);

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
