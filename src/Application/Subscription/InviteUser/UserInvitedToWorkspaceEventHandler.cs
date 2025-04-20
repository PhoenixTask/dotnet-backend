using Application.Abstractions.Emails;
using Domain.Subscriptions;
using MediatR;

namespace Application.Subscription.InviteUser;
internal sealed class UserInvitedToWorkspaceEventHandler(IEmailService emailService) : INotificationHandler<UserInvitedToWorkspaceDomainEvent>
{
    public Task Handle(UserInvitedToWorkspaceDomainEvent notification, CancellationToken cancellationToken)
    {
        if (!notification.Invitation.IsApproved)
        {
            emailService.SendEmailAsync(notification.Invitation.Invited.Email,
                "Invitation Request",
                $"You are invited to workspace {notification.Invitation.Workspace.Name} " +
                $"To Accept and join to this workspace click below link " +
                $"FrontEndProject{notification.Invitation.Token}");
        }
        return Task.CompletedTask;
    }
}
