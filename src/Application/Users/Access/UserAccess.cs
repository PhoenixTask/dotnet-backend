using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Access;

internal sealed class UserAccess(IApplicationDbContext context, IUserContext userContext) : IUserAccess
{
    public async Task<bool> IsAuthenticatedAsync(Guid workspaceId, Role? role = null)
    {
        IQueryable<TeamMember> accessCheckQuery = context.Members
            .AsNoTracking()
            .Where(x => x.UserId == userContext.UserId)
            .Where(x => x.WorkspaceId == workspaceId);

        if (role.HasValue)
        {
            accessCheckQuery = accessCheckQuery.Where(x => x.Role == role);
        }
        bool isAuthorize = await accessCheckQuery.AnyAsync();

        return isAuthorize;
    }
}
