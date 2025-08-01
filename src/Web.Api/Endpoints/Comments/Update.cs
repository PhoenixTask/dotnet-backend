using Application.Comments.Update;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Comments;

internal sealed class Update : IEndpoint
{
    public sealed record Request(string Content);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("comment/{Id}", async (Guid Id, Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateCommentCommand(Id, request.Content);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithName("Update Comment")
        .WithSummary("Update specific comment for task")
        .RequireAuthorization()
        .WithTags(Tags.Comments);
    }
}
