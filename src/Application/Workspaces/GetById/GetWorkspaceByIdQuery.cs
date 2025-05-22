using Application.Abstractions.Messaging;
using Application.Workspaces.Get;

namespace Application.Workspaces.GetById;
public sealed record GetWorkspaceByIdQuery(Guid WorkspaceId) : IQuery<WorkspaceResponse>;
