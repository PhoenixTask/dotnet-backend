using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Login;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.LoginGoogle;

internal sealed class LoginGoogleCommandHandler(IGoogleProvider googleProvider, IApplicationDbContext context, ITokenProvider tokenProvider) : ICommandHandler<LoginGoogleCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
    {
        Dictionary<string, string> claimsResult = await googleProvider.GetClaimsAsync(request.Token, cancellationToken);

        if (claimsResult is null)
        {
            return Result.Failure<LoginResponse>(UserErrors.Unauthorized);
        }

        string normalizedUserName = claimsResult["Email"].ToUpperInvariant();

        User? user = await context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == claimsResult["Email"], cancellationToken);

        if (user is null)
        {
            user = new User
            {
                Email = claimsResult["Email"],
                FirstName = claimsResult["GivenName"],
                LastName = claimsResult["FamilyName"],
                UserName = normalizedUserName,
                NormalizedUserName = normalizedUserName,
                ProfileImage = claimsResult["Picture"],
            };
            await context.Users.AddAsync(user, cancellationToken);
        }

        string token = tokenProvider.Create(user);
        string refresh = tokenProvider.GenerateRefreshToken();

        UserToken? refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);
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
