using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.RevokeRefreshToken;

internal sealed class RevokeRefreshTokenCommandHandler(IApplicationDbContext context, IUserContext userContext) : ICommandHandler<RevokeRefreshTokenCommand>
{
    public async Task<Result> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        UserToken? token = await context.RefreshTokens
            .Where(x => x.UserId == userContext.UserId)
            .Where(x => x.TokenType == TokenType.RefreshToken)
            .SingleOrDefaultAsync(x => x.Token == request.Refresh, cancellationToken);

        if (token is null)
        {
            return Result.Failure(UserErrors.Unauthorized);
        }

        context.RefreshTokens.Remove(token);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
