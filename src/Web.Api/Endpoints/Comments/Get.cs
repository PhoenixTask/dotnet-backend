using Application.Comments.Get;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Comments;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("task/{taskId}/comments", async (Guid taskId, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetTaskCommentsQuery(taskId);

            Result<List<CommentResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("List Task's Comments")
        .WithSummary("Get list of comments for a specified task")
        .RequireAuthorization()
        .WithTags(Tags.Comments);
    }
}
