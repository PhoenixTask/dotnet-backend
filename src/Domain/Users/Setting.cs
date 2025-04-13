using Domain.Interfaces;
using SharedKernel;

namespace Domain.Users;
public sealed class Setting :Entity , IAuditableEntity , IBlamableEntity
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public DateTime? CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }
    public User? CreatedBy { get; set; }
    public User? ModifiedBy { get; }
    public Guid? CreatedById { get; }
    public Guid? ModifiedById { get; }
}
