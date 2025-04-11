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
    public User? CreatedBy { get; }
    public User? ModifiedBy { get; }
}
