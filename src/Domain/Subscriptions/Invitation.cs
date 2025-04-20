using Domain.Interfaces;
using Domain.Users;
using Domain.Workspaces;
using SharedKernel;

namespace Domain.Subscriptions;
public sealed class Invitation : Entity, IAuditableEntity , IBlamableEntity
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public Workspace Workspace { get; set; }
    public User Invited { get; set; }
    public bool IsApproved { get; set; }
    public ProjectRole ProjectRole { get; set; }
    public DateTime? CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }
    public User? CreatedBy { get; set; }
    public User? ModifiedBy { get; }
    public Guid? CreatedById { get; }
    public Guid? ModifiedById { get; }
}
