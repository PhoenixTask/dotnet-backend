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

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/refresh-token", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new RefreshTokenCommnad(request.UserId, request.RefreshToken);

            Result<LoginResponse> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
