using SharedKernel;

namespace Domain.Users;

public sealed record UserRegisteredDomainEvent(User User) : IDomainEvent;
