using Domain.Interfaces;
using Domain.Projects;
using Domain.Users;
using SharedKernel;

namespace Domain.Tasks;
public sealed class Task : Entity, IAuditableEntity, IBlamableEntity, ISoftDeletableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateOnly? DeadLine { get; set; }
    public string Attachment { get; set; }
    public string Thumbnail { get; set; }
    public int Priority { get; set; }
    public int Order { get; set; }
    public Board Board { get; set; }
    public Guid BoardId { get; set; }
    public DateTime? CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }
    public DateTime? DeletedOnUtc { get; }
    public bool Deleted { get; }
    public User? CreatedBy { get; set; }
    public User? ModifiedBy { get; }
    public User? DeletedBy { get; }
    public Guid? CreatedById { get; }
    public Guid? ModifiedById { get; }
}
