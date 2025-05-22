using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Projects.Get;
using Application.Users.AccessAction;
using Domain.Projects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.GetById;

internal sealed class GetProjectByIdQueryHandler(IUserContext userContext, ISender sender, IApplicationDbContext context) : IQueryHandler<GetProjectByIdQuery, ProjectResponse>
{
    public async Task<Result<ProjectResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        bool projectExists = await context.Projects
            .AnyAsync(x => x.Id == request.ProjectId, cancellationToken);

        if (!projectExists)
        {
            return Result.Failure<ProjectResponse>(ProjectErrors.NotFound(request.ProjectId));
        }

        UserAccessCommand accessRequest = new(userContext.UserId, request.ProjectId, typeof(Project));
        Result accessResult = await sender.Send(accessRequest, cancellationToken);
        if (accessResult.IsFailure)
        {
            return Result.Failure<ProjectResponse>(accessResult.Error);
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
