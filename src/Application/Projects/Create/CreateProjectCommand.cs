using Application.Abstractions.Messaging;

namespace Application.Projects.Create;

public sealed record CreateProjectCommand(string Name, Guid WorkspaceId) : ICommand<Guid>;
