using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Projects;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.Create;

internal sealed class CreateProjectCommandHandler
    (IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<CreateProjectCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        Workspace? workspace = await context.Workspaces.SingleOrDefaultAsync(x => x.Id == request.WorkspaceId, cancellationToken);

        if (workspace is null)
        {
            return Result.Failure<Guid>(WorkspaceErrors.NotFound(request.WorkspaceId));
        }

        bool hasAccess = await userAccess.IsAuthenticatedAsync(workspace.Id);
        if (!hasAccess)
        {
            return Result.Failure<Guid>(WorkspaceErrors.NotFound(request.WorkspaceId));
        }

        var project = new Project
        {
            Name = request.Name.Trim(),
            Workspace = workspace,
            Color = request.Color
        };
        await context.Projects.AddAsync(project, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}
