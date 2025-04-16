using SharedKernel;

namespace Domain.Tasks;
public static class TaskErrors
{
    public static Error NotFound(Guid taskId) => Error.NotFound(
        "Task.NotFound",
        $"The task with the Id = '{taskId}' was not found");
    public static readonly Error ExpiredDeadLine = Error.Failure(
        "Task.ExpiredDeadLine",
        $"The deadline is passed and cannot be assigned");
}
