using Domain.Interfaces;
using Domain.Users;
using SharedKernel;
using Domain.Projects;

namespace Domain.Workspaces;
public sealed class Workspace : Entity , ISoftDeletableEntity , IAuditableEntity , IBlamableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public DateTime? CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }
    public DateTime? DeletedOnUtc { get; }
    public bool Deleted { get; }
    public User? DeletedBy { get; }
    public User? CreatedBy { get; }
    public User? ModifiedBy { get; }
    public ICollection<Project> Projects { get; set; }
}
