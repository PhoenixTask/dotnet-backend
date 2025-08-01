using Application.Abstractions.Messaging;

namespace Application.Tasks.GetTaskByDate;
public sealed record GetTaskByDeadLineQuery(Guid ProjectId, DateTime StartDate, DateTime EndDate) : IQuery<List<TaskResponse>>;
