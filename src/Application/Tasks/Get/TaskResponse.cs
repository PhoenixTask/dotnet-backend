namespace Application.Tasks.Get;

public sealed class TaskResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? DeadLine { get; set; }
    public int Priority { get; set; }
    public int Order { get; set; }
    public bool IsComplete { get; set; }
}
