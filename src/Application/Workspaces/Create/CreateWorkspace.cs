using Application.Abstractions.Messaging;

namespace Application.Workspaces.Create;
public sealed record CreateWorkspaceCommand(string Name,string Color) : ICommand<Guid>;
