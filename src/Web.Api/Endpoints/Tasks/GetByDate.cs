using Application.Tasks.GetTaskByDate;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Tasks;

internal sealed class GetByDate : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("task/get-by-deadline", async (Guid ProjectId, DateTime Start, DateTime End, ISender sender, CancellationToken cancellationToken, bool IncludeCompleted = true) =>
        {
            var query = new GetTaskByDeadLineQuery(ProjectId, Start, End, IncludeCompleted);

            Result<List<TaskResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithName("Get Tasks By Date Range")
        .WithSummary("Get tasks within a date range for a project")
        .RequireAuthorization()
        .WithTags(Tags.Tasks);
    }
}
