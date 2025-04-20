using SharedKernel;

namespace Domain.Subscriptions;
public static class TeamMemberErrors
{
    public static readonly Error AlreadyMember = Error.Conflict(
        "TeamMember.AlreadyMember",
        "The user already participant in this workspace");
}
