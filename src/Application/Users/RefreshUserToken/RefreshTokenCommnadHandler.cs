using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Login;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.RefreshUserToken;

internal sealed class RefreshTokenCommnadHandler(
    IApplicationDbContext context,
    ITokenProvider tokenProvider) : ICommandHandler<RefreshTokenCommnad, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommnad request, CancellationToken cancellationToken)
    {
        UserToken? refreshToken = await context.RefreshTokens
            .Include(x=>x.User)
            .SingleOrDefaultAsync(x=>x.UserId == request.UserId && x.Token == request.RefreshToken && x.TokenType == TokenType.RefreshToken,cancellationToken);
        if(refreshToken is null)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidToken);
        }

        if (refreshToken.ExpireOnUtc < DateTime.UtcNow)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidToken);
        }

        string token = tokenProvider.Create(refreshToken.User);
        string refresh = tokenProvider.GenerateRefreshToken();
        
        refreshToken.Token = refresh;
        refreshToken.ExpireOnUtc = DateTime.UtcNow.AddDays(7);

        await context.SaveChangesAsync(cancellationToken);

        var response = new LoginResponse { RefreshToken = refresh, Token = token, UserId = refreshToken.User.Id };
        return response;
    }
}
