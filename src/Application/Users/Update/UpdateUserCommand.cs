using Application.Abstractions.Messaging;
namespace Application.Users.Update;
public sealed record UpdateUserCommand(string? FirstName, string? LastName) : ICommand;
