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

        Project? project = await context.Projects
            .Include(x => x.Boards)
            .ThenInclude(x => x.Tasks)
            .Where(x => x.Id == request.ProjectId && x.CreatedById == userId)
            .SingleOrDefaultAsync(cancellationToken);

        if (project is null)
        {
            return Result.Failure(ProjectErrors.NotFound(request.ProjectId));
        }

        context.Projects.Remove(project);

        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
