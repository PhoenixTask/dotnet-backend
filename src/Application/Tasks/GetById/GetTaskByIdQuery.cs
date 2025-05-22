using Application.Abstractions.Messaging;
using Application.Tasks.Get;

namespace Application.Tasks.GetById;
public sealed record GetTaskByIdQuery(Guid TaskId) : IQuery<TaskResponse>;
