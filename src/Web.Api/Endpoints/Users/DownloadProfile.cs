using Application.Users.DownloadProfile;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using SharedKernel;

namespace Web.Api.Endpoints.Users;

internal sealed class DownloadProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("user/download", static async (Guid userId, IContentTypeProvider contentTypeProvider, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DownloadProfileCommand(userId);

            Result<string> result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Results.NoContent();
            }
            FileStream stream = File.OpenRead(result.Value);
            contentTypeProvider.TryGetContentType(result.Value, out string contentType);
            return Results.File(stream, contentType, $"{userId}.{Path.GetExtension(result.Value)}");
        })
        .WithName("Download Profile Picture")
        .WithSummary("Download user profile picture")
        //.RequireAuthorization()
        .WithTags(Tags.Users);
    }
}
