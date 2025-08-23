using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetSetting;

internal sealed class GetSettingQueryHandler(
    IApplicationDbContext context, IUserContext userContext) : IQueryHandler<GetSettingQuery, string>
{
    public async Task<Result<string>> Handle(GetSettingQuery request, CancellationToken cancellationToken)
    {
        Setting setting = await context.Settings
            .SingleOrDefaultAsync(x => x.CreatedById == userContext.UserId && x.Key == request.Key, cancellationToken);
        if (setting is null)
        {
            return Result.Failure<string>(UserErrors.SettingNotFound);
        }
        return setting.Value;
    }
}
