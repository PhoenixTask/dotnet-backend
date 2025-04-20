using SharedKernel;

namespace Domain.Subscriptions;
public static class InvitationErrors
{
    public static readonly Error RequestSentBefore = Error.Conflict(
         "Invitation.RequestSentBefore",
         "The invitation already sent to user");

    public static readonly Error InviteMyself = Error.Conflict(
         "Invitation.InviteMyself",
         "You are already access this workspaces");
    public static Error NotFound => Error.NotFound(
        "Invitation.NotFound",
        $"No invitation found");
}
