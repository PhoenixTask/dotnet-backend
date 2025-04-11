namespace Domain.Interfaces;

public interface IAuditableEntity
{
    DateTime? CreatedOnUtc { get; }
    DateTime? ModifiedOnUtc { get; }
}
