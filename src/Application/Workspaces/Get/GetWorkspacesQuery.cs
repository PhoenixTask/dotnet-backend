using Application.Abstractions.Messaging;

namespace Application.Workspaces.Get;
public sealed record GetWorkspacesQuery(int Page, int PageSize) : IQuery<List<WorkspaceResponse>>;
