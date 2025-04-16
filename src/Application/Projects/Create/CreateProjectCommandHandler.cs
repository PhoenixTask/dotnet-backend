using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.Create;

internal sealed class CreateProjectCommandHandler
    (IApplicationDbContext context, IUserContext userContext) : ICommandHandler<CreateProjectCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        Workspace? workspace = await context.Workspaces.SingleOrDefaultAsync(x=>x.Id == request.WorkspaceId, cancellationToken);

        if (workspace is null)
        {
            return Result.Failure<Guid>(WorkspaceErrors.NotFound(request.WorkspaceId));
        }

        //TODO: Check user can add project
        Guid userId = userContext.UserId;
        if (workspace.CreatedById != userId)
        {
            return Result.Failure<Guid>(WorkspaceErrors.NotFound(request.WorkspaceId));
        }

        var project = new Project
        {
            Name = request.Name.Trim(),
            Workspace = workspace
        };
        await context.Projects.AddAsync(project, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}
