using Application.Abstractions.Messaging;

namespace Application.Tasks.GetTaskByDate;
public sealed record GetTaskByDeadLineQuery(Guid ProjectId, DateTime StartDate, DateTime EndDate, bool IncludeCompleted) : IQuery<List<TaskResponse>>;
