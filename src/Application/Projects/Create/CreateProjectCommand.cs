using Application.Abstractions.Messaging;

namespace Application.Projects.Create;

public sealed record CreateProjectCommand(string Name,string Color, Guid WorkspaceId) : ICommand<Guid>;
