using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.Delete;

internal sealed class DeleteProjectCommandHandler(
    IApplicationDbContext context, IUserContext userContext) : ICommandHandler<DeleteProjectCommand>
{
    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;

        int countDeleted = await context.Projects
            .Where(x => x.Id == request.ProjectId && x.CreatedById == userId)
            .ExecuteDeleteAsync(cancellationToken);

        return countDeleted > 0 ? Result.Success() : Result.Failure(ProjectErrors.NotFound(request.ProjectId))
    }
}
