using System.Buffers.Text;
using FluentValidation;

namespace Application.Users.UploadProfile;

internal sealed class UploadProfileCommandValidator : AbstractValidator<UploadProfileCommand>
{
    public UploadProfileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.Base64File).NotEmpty().WithMessage("File is empty")
            .Must(x=>Base64.IsValid(x)).WithMessage("File format should be base64 (remove data:image/jpeg;base64)");

        RuleFor(x => x.FileName).NotEmpty()
            .Must(x => x.Split('.').Length == 2).WithMessage("FileName should have extention defined");
    }
}
