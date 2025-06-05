using Domain.Interfaces;
using Domain.Users;
using SharedKernel;

namespace Domain.Projects;
public sealed class Board : Entity , IAuditableEntity,IBlamableEntity, ISoftDeletableEntity , IOrderable
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public int Order { get; set; }
    public bool IsArchive { get; set; }
    public DateTime? CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }
    public DateTime? DeletedOnUtc { get; }
    public bool Deleted { get; }
    public Project Project { get; set; }
    public Guid ProjectId { get; set; }
    public User? CreatedBy { get; set; }
    public User? ModifiedBy { get; }
    public User? DeletedBy { get; }
    public Guid? CreatedById { get; }
    public Guid? ModifiedById { get; }
    public ICollection<Tasks.Task> Tasks { get; set; }
}
