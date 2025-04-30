using Application.Abstractions.Messaging;

namespace Application.Workspaces.Get;
public sealed record GetWorkspacesQuery: IQuery<List<WorkspaceResponse>>;
