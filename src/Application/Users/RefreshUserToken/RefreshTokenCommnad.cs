using Application.Abstractions.Messaging;
using Application.Users.Login;

namespace Application.Users.RefreshUserToken;
public sealed record RefreshTokenCommnad(Guid UserId,string RefreshToken) : ICommand<LoginResponse>;
