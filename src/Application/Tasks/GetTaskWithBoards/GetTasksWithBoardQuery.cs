using Application.Abstractions.Messaging;
using Application.Tasks.GetTaskByDate;

namespace Application.Tasks.GetTaskWithBoards;
public sealed record GetTasksWithBoardQuery(Guid ProjectId) : IQuery<List<TaskResponse>>;
