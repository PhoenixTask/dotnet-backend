using Application.Abstractions.Messaging;

namespace Application.Users.Login;
public sealed record LoginWithUsernameCommand(string Username, string Password) : ICommand<LoginResponse>;
