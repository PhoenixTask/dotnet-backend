using Domain.Projects;
using Domain.Users;

namespace Domain.Interfaces;
public interface ISoftDeletableEntity
{
    DateTime? DeletedOnUtc { get; }
    bool Deleted { get; }
    User? DeletedBy { get; }
}
