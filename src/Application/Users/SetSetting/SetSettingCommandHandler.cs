using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.SetSetting;

internal sealed class SetSettingCommandHandler
    (IApplicationDbContext context, IUserContext userContext) : ICommandHandler<SetSettingCommand>
{
    public async Task<Result> Handle(SetSettingCommand request, CancellationToken cancellationToken)
    {
        User? user = await context.Users.SingleOrDefaultAsync(x => x.Id == userContext.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(userContext.UserId));
        }
        string key = request.Key.Trim();
        string value = request.Value.Trim();
        Setting? setting = await context.Settings
            .SingleOrDefaultAsync(x => x.Key == key, cancellationToken);

        if (setting is null)
        {
            setting = new Setting
            {
                DisplayName = request.Key,
                Key = key,
                Value = value
            };

            await context.Settings.AddAsync(setting, cancellationToken);
        }
        else
        {
            setting.Value = value;
        }
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
