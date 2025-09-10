using Application.Abstractions.Messaging;
using Application.Users.Login;

namespace Application.Users.LoginGoogle;
public sealed record LoginGoogleCommand(string Token) : ICommand<LoginResponse>;
