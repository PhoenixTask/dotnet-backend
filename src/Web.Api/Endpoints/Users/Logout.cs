using Application.Users.RevokeRefreshToken;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/logout", async (ISender sender, HttpContext context, CancellationToken cancellationToken) =>
        {
            if (!context.User.Identity?.IsAuthenticated ?? false)
            {
                return Results.NoContent();
            }
            var command = new RevokeRefreshTokenCommand(CookieService.GetRefreshToken(context));

            Result result = await sender.Send(command, cancellationToken);
            if (result.IsSuccess)
            {
                CookieService.ExpireToken(context);
            }
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Logout User")
        .WithSummary("Remove user credential from cookie and expire refresh token")
        .WithTags(Tags.Users);
    }
}
