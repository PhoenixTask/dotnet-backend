namespace Application.Tasks.GetTaskByDate;

public class TaskResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DeadLine { get; set; }
    public int Priority { get; set; }
    public int Order { get; set; }
    public bool IsComplete { get; set; }

    public Guid BoardId { get; set; }
    public string BoardName { get; set; }
}
