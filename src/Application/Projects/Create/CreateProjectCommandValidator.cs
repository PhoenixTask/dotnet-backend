using FluentValidation;

namespace Application.Projects.Create;

internal sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.WorkspaceId).NotEmpty();
    }
}
