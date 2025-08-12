using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Subscriptions;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Workspaces.Update;

internal sealed class UpdateWorkspaceCommandHandler(IApplicationDbContext context, IUserAccess userAccess)
    : ICommandHandler<UpdateWorkspaceCommand>
{
    public async Task<Result> Handle(UpdateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Workspace? workspace = await context.Members
            .Include(x => x.Workspace)
            .Select(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        bool hasOwnerAccess = await userAccess.IsAuthenticatedAsync(request.Id, Role.Owner);

        if (workspace is null || !hasOwnerAccess)
        {
            return Result.Failure(WorkspaceErrors.NotFound(request.Id));
        }

        workspace.Name = request.Name.Trim();
        workspace.Color = request.Color.Trim();

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
