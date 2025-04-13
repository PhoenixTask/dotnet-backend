using Domain.Users;

namespace Domain.Interfaces;
public interface IBlamableEntity
{
    User? CreatedBy { get; }
    Guid? CreatedById { get; }
    User? ModifiedBy { get; }
    Guid? ModifiedById { get; }
}
