using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetSettings;

internal sealed class GetSettingsQueryHandler(
    IApplicationDbContext context, IUserContext userContext) : IQueryHandler<GetSettingsQuery, Dictionary<string, string>>
{
    public async Task<Result<Dictionary<string, string>>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        Dictionary<string, string> settings = await context.Settings
            .Where(x => x.CreatedById == userContext.UserId).ToDictionaryAsync(x => x.Key, x => x.Value, cancellationToken);
        return settings;
    }
}
