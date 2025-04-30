using Application.Abstractions.Messaging;

namespace Application.Tasks.Get;
public sealed record GetTasksQuery(Guid BoardId) : IQuery<List<TaskResponse>>;
