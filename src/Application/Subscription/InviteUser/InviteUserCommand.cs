using Application.Abstractions.Messaging;
using Domain.Subscriptions;

namespace Application.Subscription.InviteUser;
public sealed record InviteUserCommand(string Email , Guid WorkspaceId,Role Role) : ICommand;
