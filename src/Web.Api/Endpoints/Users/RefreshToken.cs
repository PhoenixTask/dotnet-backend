using Application.Users.Login;
using Application.Users.RefreshUserToken;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class RefreshToken : IEndpoint
{
    public sealed record Request(Guid UserId, string RefreshToken);
    public sealed record Request2(Guid UserId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/refresh-token", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new RefreshTokenCommnad(request.UserId, request.RefreshToken);

            Result<LoginResponse> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Refresh Token")
        .WithSummary("Refresh user authentication token")
        .WithTags(Tags.Users);

        app.MapPost("user/refresh-token", async (Request2 request, ISender sender, HttpContext context, CancellationToken cancellationToken) =>
        {
            var command = new RefreshTokenCommnad(request.UserId, CookieService.GetRefreshToken(context));

            Result<LoginResponse> result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }
            CookieService.SetToken(result.Value.Token, result.Value.RefreshToken, context);
            return Results.NoContent();
        })
            .WithName("Refresh Token (http only cookie)")
            .HasApiVersion(2)
            .WithSummary("Refresh user authentication token")
            .WithTags(Tags.Users);
    }
}
