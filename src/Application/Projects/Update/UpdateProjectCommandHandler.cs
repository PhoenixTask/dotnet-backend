using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.Update;

internal sealed class UpdateProjectCommandHandler(
    IApplicationDbContext context , IUserContext userContext) : ICommandHandler<UpdateProjectCommand, Result>
{
    public async Task<Result<Result>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;

        Project? project =await context.Projects.SingleOrDefaultAsync(x => x.Id == request.ProjectId && x.CreatedById == userId,cancellationToken);

        if(project is null)
        {
            return Result.Failure(ProjectErrors.NotFound(request.ProjectId));
        }

        project.Name = request.Name;
        project.Color = request.Color;
        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
