using Application.Abstractions.Messaging;

namespace Application.Projects.Get;

public sealed record GetProjectsQuery(Guid WorkspaceId) : IQuery<List<ProjectResponse>>;
