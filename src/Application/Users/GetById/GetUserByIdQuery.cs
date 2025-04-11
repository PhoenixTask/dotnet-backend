using Application.Abstractions.Messaging;

namespace Application.Users.GetById;
//TODO: Check Code ! 
public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
