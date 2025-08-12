using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Subscriptions;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Workspaces.Delete;

internal sealed class DeleteWorkspaceCommandHandler(IApplicationDbContext context, IUserContext userContext, IUserAccess userAccess)
    : ICommandHandler<DeleteWorkspaceCommand>
{
    public async Task<Result> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        Workspace? workspace = await context.Workspaces
            .Include(x => x.Projects)
            .ThenInclude(x => x.Boards)
            .ThenInclude(x => x.Tasks)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        bool hasOwnerAccess = await userAccess.IsAuthenticatedAsync(request.Id, Role.Owner);

        if (workspace is null || !hasOwnerAccess)
        {
            return Result.Failure(WorkspaceErrors.NotFound(request.Id));
        }

        TeamMember member = await context.Members
            .Where(x => x.WorkspaceId == request.Id && x.UserId == userContext.UserId)
            .SingleAsync(cancellationToken);

        context.Members.Remove(member);

        context.Workspaces.Remove(workspace);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
