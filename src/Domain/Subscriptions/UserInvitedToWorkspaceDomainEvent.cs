using SharedKernel;

namespace Domain.Subscriptions;
public sealed record UserInvitedToWorkspaceDomainEvent(Invitation Invitation) : IDomainEvent;
