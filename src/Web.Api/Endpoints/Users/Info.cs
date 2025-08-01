using Application.Abstractions.Authentication;
using Application.Users.GetById;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class Info : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("user/info", async (IUserContext userContext, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetUserByIdQuery(userContext.UserId);

            Result<UserResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Get User Info")
        .WithSummary("Get current user information")
        .RequireAuthorization()
        .WithTags(Tags.Users);
    }
}
