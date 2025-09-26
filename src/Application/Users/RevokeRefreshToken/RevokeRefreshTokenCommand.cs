using Application.Abstractions.Messaging;

namespace Application.Users.RevokeRefreshToken;
public sealed record RevokeRefreshTokenCommand(string Refresh) : ICommand;
