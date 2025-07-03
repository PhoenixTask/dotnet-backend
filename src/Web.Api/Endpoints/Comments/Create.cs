using Application.Comments.Create;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Comments;

internal sealed class Create : IEndpoint
{
    public sealed record Request(Guid TaskId, string Comment);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("comment/", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateCommentCommand(request.TaskId, request.Comment);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Comments);
    }
}
