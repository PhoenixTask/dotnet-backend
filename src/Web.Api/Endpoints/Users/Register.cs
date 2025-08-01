using Application.Users.Register;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class Register : IEndpoint
{
    public sealed record Request(string Username , string Email, string Password,string FirstName,string LastName);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand(
                request.Username,
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Register User")
        .WithSummary("Create a new user account")
        .WithTags(Tags.Users);
    }
}
