using Application.Abstractions.Messaging;

namespace Application.Tasks.Create;
public sealed class CreateTaskCommand : ICommand<Guid>
{
    public string Name { get; set; }
    public Guid BoardId { get; set; }
    public string Description { get; set; }
    public DateTime? DeadLine { get; set; }
    public int Priority { get; set; }
}
