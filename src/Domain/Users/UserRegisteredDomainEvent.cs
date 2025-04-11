using SharedKernel;

namespace Domain.Users;

public sealed record UserRegisteredDomainEvent(Guid UserId) : IDomainEvent;
