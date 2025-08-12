using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Projects.Get;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.GetById;

internal sealed class GetProjectByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetProjectByIdQuery, ProjectResponse>
{
    public async Task<Result<ProjectResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        bool projectExists = await context.Projects
            .AnyAsync(x => x.Id == request.ProjectId, cancellationToken);

        if (!projectExists)
        {
            return Result.Failure<ProjectResponse>(ProjectErrors.NotFound(request.ProjectId));
        }

        return await context.Projects
            .AsNoTracking()
            .Select(x => new ProjectResponse
            {
                Id = x.Id,
                Color = x.Color,
                Name = x.Name
            })
            .SingleAsync(x => x.Id == request.ProjectId, cancellationToken);
    }
}
