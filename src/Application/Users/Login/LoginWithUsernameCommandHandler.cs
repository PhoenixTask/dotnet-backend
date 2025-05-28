using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Login;

internal sealed class LoginWithUsernameCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider
    ) : ICommandHandler<LoginWithUsernameCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginWithUsernameCommand request, CancellationToken cancellationToken)
    {
        string normalizedUserName = request.Username.ToUpperInvariant();

        User? user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);

        if (user is null)
        {
            return Result.Failure<LoginResponse>(UserErrors.NotFoundByUserName);
        }

        bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!verified)
        {
            return Result.Failure<LoginResponse>(UserErrors.NotFoundByUserName);
        }

        string token = tokenProvider.Create(user);
        string refresh = tokenProvider.GenerateRefreshToken();

        RefreshToken? refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);
        if (refreshToken is null)
        {
            refreshToken = new RefreshToken
            {
                UserId = user.Id,
            };
            await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }
        refreshToken.Token = refresh;
        refreshToken.ExpireOnUtc = DateTime.UtcNow.AddDays(7);

        await context.SaveChangesAsync(cancellationToken);
        var response = new LoginResponse { RefreshToken = refresh , Token = token, UserId = user.Id };
        return response;
    }
}
