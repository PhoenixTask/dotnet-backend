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
        Guid userId = userContext.UserId;
        User? user = await context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(userId));
        }

        Setting? setting = await context.Settings.SingleOrDefaultAsync(x => x.Key == request.Key.Trim(), cancellationToken);
        if (setting is null)
        {
            setting = new Setting
            {
                DisplayName = request.Key,
                Key = request.Key.Trim(),
                Value = request.Value.Trim()
            };

            await context.Settings.AddAsync(setting, cancellationToken);
        }
        else
        {
            setting.Value = request.Value.Trim();
        }
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
