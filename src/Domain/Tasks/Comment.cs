using Domain.Interfaces;
using Domain.Users;
using SharedKernel;

namespace Domain.Tasks;
public sealed class Comment : Entity, IBlamableEntity, IAuditableEntity, ISoftDeletableEntity
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Task Task { get; set; }
    public Guid TaskId { get; set; }

    public User? CreatedBy { get; }

    public Guid? CreatedById { get; }
    public User? ModifiedBy { get; }

    public Guid? ModifiedById { get; }

    public DateTime? CreatedOnUtc { get; }

    public DateTime? ModifiedOnUtc { get; }

    public DateTime? DeletedOnUtc { get; }

    public bool Deleted { get; }

    public User? DeletedBy { get; }
}
