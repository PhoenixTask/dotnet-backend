using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Login;

internal sealed class LoginCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider
    ) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        string normalizedUserName = request.Username.ToUpperInvariant();

        User? user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);

        if (user is null)
        {
            return Result.Failure<LoginResponse>(UserErrors.NotFoundByUserName);
        }

        bool verified = passwordHasher.Verify(request.Password, user.PasswordHash ?? string.Empty);

        if (!verified)
        {
            return Result.Failure<LoginResponse>(UserErrors.NotFoundByUserName);
        }

        string token = tokenProvider.Create(user);
        string refresh = tokenProvider.GenerateRefreshToken();

        UserToken? refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id && x.TokenType == TokenType.RefreshToken, cancellationToken);
        if (refreshToken is null)
        {
            refreshToken = new UserToken
            {
                UserId = user.Id,
                TokenType = TokenType.RefreshToken
            };
            await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }
        refreshToken.Token = refresh;
        refreshToken.ExpireOnUtc = DateTime.UtcNow.AddDays(7);

        await context.SaveChangesAsync(cancellationToken);
        var response = new LoginResponse(user.Id, token, refresh);
        return response;
    }
}
