using Domain.Subscriptions;

namespace Application.Users.Access;
internal interface IUserAccess
{
    Task<bool> IsAuthenticatedAsync(Guid workspaceId, Role? role = null);
}
