namespace Application.Tasks.ChangeOrderBoard;

public sealed record TaskRequest(Guid TaskId, Guid BoardId, int Order);
