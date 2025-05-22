using Application.Abstractions.Messaging;
using Application.Projects.Get;

namespace Application.Projects.GetById;
public sealed record GetProjectByIdQuery(Guid ProjectId) : IQuery<ProjectResponse>;
