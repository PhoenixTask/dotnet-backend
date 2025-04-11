using Application.Abstractions.Messaging;

namespace Application.Users.Login;
//TODO: Check Code ! 
public sealed record LoginUserCommand(string Email, string Password) : ICommand<string>;
