using Domain.Users;

namespace Domain.Interfaces;
public interface IBlamableEntity
{
    User? CreatedBy { get; }
    User? ModifiedBy { get; }
}
