using Application.Users.Login;
using Application.Users.LoginGoogle;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class GoogleLogin : IEndpoint
{
    public sealed record Request(string TokenId);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/google-login", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new LoginGoogleCommand(request.TokenId);

            Result<LoginResponse> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
       .WithName("Login User By Google Provider")
       .WithSummary("Authenticate user with google")
       .WithTags(Tags.Users);


        app.MapPost("user/google-login", async (Request request, ISender sender, HttpContext context, CancellationToken cancellationToken) =>
        {
            var command = new LoginGoogleCommand(request.TokenId);

            Result<LoginResponse> result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }
            CookieService.SetToken(result.Value.Token, result.Value.RefreshToken, context);
            return Results.NoContent();
        })
            .HasApiVersion(2)
      .WithName("Login User By Google Provider (http only cookie)")
      .WithSummary("Authenticate user with google")
      .WithTags(Tags.Users);
    }
}
