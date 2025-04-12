using Application.Abstractions.Messaging;

namespace Application.Users.Login;

public sealed record LoginUserCommand(string Username, string Password) : ICommand<string>;
