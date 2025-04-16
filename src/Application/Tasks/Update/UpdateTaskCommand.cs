using Application.Abstractions.Messaging;

namespace Application.Tasks.Update;
public sealed class UpdateTaskCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid BoardId { get; set; }
    public string Description { get; set; }
    public DateTime DeadLine { get; set; }
    public int Order { get; set; }
    public int Priority { get; set; }
}
