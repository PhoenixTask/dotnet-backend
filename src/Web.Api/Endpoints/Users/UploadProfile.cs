using Application.Abstractions.Authentication;
using Application.Users.UploadProfile;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Users;

internal sealed class UploadProfile : IEndpoint
{
    public sealed record Request(string Base64File,string FileName);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("user/upload", async (Request request, IUserContext userContext, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UploadProfileCommand
            {
                Base64File = request.Base64File,
                UserId =userContext.UserId,
                FileName = request.FileName,
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Users);
    }
}
