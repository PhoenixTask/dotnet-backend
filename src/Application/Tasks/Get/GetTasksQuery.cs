using Application.Abstractions.Messaging;

namespace Application.Tasks.Get;
public sealed record GetTasksQuery(int Page, int PageSize, Guid BoardId) : IQuery<List<TaskResponse>>;
