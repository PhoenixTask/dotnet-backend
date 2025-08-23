using Application.Abstractions.Messaging;

namespace Application.Users.GetById;
public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
