using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.AccessAction;
using Domain.Projects;
using Domain.Subscriptions;
using Domain.Workspaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Projects.Create;

internal sealed class CreateProjectCommandHandler
    (IApplicationDbContext context, IUserContext userContext,ISender sender) : ICommandHandler<CreateProjectCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        Workspace? workspace = await context.Workspaces.SingleOrDefaultAsync(x=>x.Id == request.WorkspaceId, cancellationToken);

        if (workspace is null)
        {
            return Result.Failure<Guid>(WorkspaceErrors.NotFound(request.WorkspaceId));
        }

        UserAccessCommand accessRequest = new(userContext.UserId, workspace.Id, typeof(Workspace), ProjectRole.Manager);
        Result accessResult = await sender.Send(accessRequest, cancellationToken);
        if (accessResult.IsFailure)
        {
            return Result.Failure<Guid>(accessResult.Error);
        }

        var project = new Project
        {
            Name = request.Name.Trim(),
            Workspace = workspace,
            Color = request.Color
        };
        await context.Projects.AddAsync(project, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}
