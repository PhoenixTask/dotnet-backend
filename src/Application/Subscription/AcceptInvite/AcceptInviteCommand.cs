using Application.Abstractions.Messaging;

namespace Application.Subscription.AcceptInvite;
public sealed record AcceptInviteCommand(string Token) : ICommand;
