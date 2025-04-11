namespace Infrastructure.Authorization;

internal sealed class PermissionProvider
{
    public Task<HashSet<string>> GetForUserIdAsync(Guid userId)
    {
        // TODO: Here you'll implement your logic to fetch permissions.
        HashSet<string> permissionsSet = [];

        return Task.FromResult(permissionsSet);
    }
}
