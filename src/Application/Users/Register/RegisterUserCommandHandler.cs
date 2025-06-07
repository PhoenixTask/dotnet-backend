using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Register;

internal sealed class RegisterUserCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        string email = command.Email.ToLower(System.Globalization.CultureInfo.CurrentCulture);
        if (await context.Users.AnyAsync(u => u.Email == email, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }

        string userName = command.Username.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
        if (await context.Users.AnyAsync(u => u.NormalizedUserName == userName, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.UsernameNotUnique);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = command.Username,
            NormalizedUserName = userName,
            PasswordHash = passwordHasher.Hash(command.Password),
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        user.Raise(new UserRegisteredDomainEvent(user.Id));

        context.Users.Add(user);

        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
