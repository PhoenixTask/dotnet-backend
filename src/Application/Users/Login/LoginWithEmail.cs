using Application.Abstractions.Messaging;

namespace Application.Users.Login;
public sealed record LoginCommand(string Username, string Password) : ICommand<LoginResponse>;
