using Application.Abstractions.Messaging;

namespace Application.Projects.Get;

public sealed record GetProjectsQuery(int Page , int PageSize,Guid WorkspaceId) : IQuery<List<ProjectResponse>>;
