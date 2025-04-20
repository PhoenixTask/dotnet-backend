using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Subscriptions;
using Domain.Users;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Subscription.InviteUser;

internal sealed class InviteUserCommandHandler(
    IApplicationDbContext context, IUserContext userContext) : ICommandHandler<InviteUserCommand>
{
    public async Task<Result> Handle(InviteUserCommand request, CancellationToken cancellationToken)
    {
        Workspace? workspace = await context.Workspaces.SingleOrDefaultAsync(x =>
        x.Id == request.WorkspaceId && x.CreatedById == userContext.UserId, cancellationToken);

        if (workspace is null)
        {
            return Result.Failure(WorkspaceErrors.NotFound(request.WorkspaceId));
        }

        User? invited = await context.Users.SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if (invited is null)
        {
            return Result.Failure(UserErrors.NotFoundByEmail);
        }
        if(invited.Id == userContext.UserId)
        {
            return Result.Failure(InvitationErrors.InviteMyself);
        }

        TeamMember? teamMember = await context.Members
            .SingleOrDefaultAsync(x => x.Workspace == workspace && x.User == invited, cancellationToken);

        if (teamMember is not null)
        {
            return Result.Failure(TeamMemberErrors.AlreadyMember);
        }

        Invitation? invitation = await context.Invitations
                .SingleOrDefaultAsync(x =>
                x.Invited == invited && x.Workspace == workspace
                && x.CreatedOnUtc >= DateTime.UtcNow.AddDays(-3), cancellationToken);

        if (invitation is not null)
        {
            return Result.Failure(InvitationErrors.RequestSentBefore);
        }
        invitation = new Invitation
        {
            Invited = invited,
            Workspace = workspace,
            ProjectRole = request.Role,
            Token = Guid.NewGuid().ToString(),
        };
        invitation.Raise(new UserInvitedToWorkspaceDomainEvent(invitation));

        await context.Invitations.AddAsync(invitation,cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
