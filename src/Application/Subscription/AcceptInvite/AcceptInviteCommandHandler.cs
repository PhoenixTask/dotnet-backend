using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Subscriptions;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Subscription.AcceptInvite;

internal sealed class AcceptInviteCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<AcceptInviteCommand>
{
    public async Task<Result> Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
    {
        User? user = await context.Users.SingleOrDefaultAsync(x => x.Id == userContext.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFoundByUserName);
        }
        Invitation? invitation = await context.Invitations
            .Include(x => x.Workspace)
            .FirstOrDefaultAsync(x => !x.IsApproved && x.Invited == user && x.Token == request.Token, cancellationToken);

        if (invitation is null)
        {
            return Result.Failure(InvitationErrors.NotFound);
        }

        TeamMember? teamMember = await context.Members.SingleOrDefaultAsync(x => x.User == user, cancellationToken);

        if (teamMember is not null)
        {
            return Result.Failure(TeamMemberErrors.AlreadyMember);
        }

        teamMember = new TeamMember
        {
            User = user,
            Role = invitation.ProjectRole,
            Workspace = invitation.Workspace
        };

        context.Members.Add(teamMember);
        invitation.IsApproved = true;

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
