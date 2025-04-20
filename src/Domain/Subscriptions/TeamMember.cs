using Domain.Interfaces;
using Domain.Users;
using Domain.Workspaces;
using SharedKernel;

namespace Domain.Subscriptions;
public sealed class TeamMember : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public User User { get; set; }
    public Guid UserId { get; set; }
    public ProjectRole Role { get; set; }
    public Workspace Workspace { get; set; }
    public Guid WorkspaceId { get; set; }
    public DateTime? CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }
    public DateTime? DeletedOnUtc { get; }
    public bool Deleted { get; }
    public User? DeletedBy { get; }
}
