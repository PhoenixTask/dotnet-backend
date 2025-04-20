using System.Text.RegularExpressions;
using FluentValidation;

namespace Application.Users.SetSetting;

internal sealed class SetSettingCommandValidator : AbstractValidator<SetSettingCommand>
{
    public SetSettingCommandValidator()
    {
        RuleFor(x => x.Key).NotEmpty().Matches(new Regex("^(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$"));
        RuleFor(x => x.Value).NotEmpty();
    }
}
