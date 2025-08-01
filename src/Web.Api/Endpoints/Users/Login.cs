using Application.Users.Login;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record Request(string Username, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/login", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(request.Username, request.Password);

            Result<string> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Login User")
        .WithSummary("Authenticate user with username and password")
        .WithTags(Tags.Users);

        app.MapPost("user/login", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new LoginWithUsernameCommand(request.Username, request.Password);

            Result<LoginResponse> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasApiVersion(2)
        .WithName("Login User v2")
        .WithSummary("Authenticate user with username and password (returns refresh token)")
        .WithTags(Tags.Users);
    }
}
