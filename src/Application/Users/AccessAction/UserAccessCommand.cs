using System;
using System.Linq.Expressions;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Domain.Subscriptions;
using Domain.Users;
using Domain.Workspaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.AccessAction;
internal sealed record UserAccessCommand(Guid UserId, Guid ModelId, Type ModelType, ProjectRole? role = null) : ICommand;
internal sealed class UserAccessCommandHandler(IApplicationDbContext context) : ICommandHandler<UserAccessCommand>
{
    public async Task<Result> Handle(UserAccessCommand request, CancellationToken cancellationToken)
    {
        bool result = request.ModelType switch
        {
            Type type when type == typeof(Workspace) => await HasWorkspaceAccess(request.UserId, request.ModelId, cancellationToken),
            Type type when type == typeof(Project) => await HasAccessToProject(request.UserId, request.ModelId, cancellationToken),
            Type type when type == typeof(Board) => await HasAccessToBoard(request.UserId, request.ModelId, cancellationToken),
            Type type when type == typeof(Domain.Tasks.Task) => await HasAccessToTask(request.UserId, request.ModelId, cancellationToken),
            _ => false
        };
        return result ? Result.Success() : Result.Failure(UserErrors.InvalidPermission);
    }
    private async Task<bool> HasWorkspaceAccess(Guid userId, Guid workspaceId, CancellationToken cancellationToken)
    {
        // Check if the user is the creator of the workspace
        Workspace workspace = await context.Workspaces.SingleAsync(x => x.Id == workspaceId, cancellationToken);
        if (workspace.CreatedById == userId)
        {
            return true; // Creator always has access
        }

        // Check if the user has access rights
        return await context.Members
            .AnyAsync(a => a.UserId == userId && a.WorkspaceId == workspaceId, cancellationToken);
    }

    public async Task<bool> HasAccessToProject(Guid userId, Guid projectId, CancellationToken cancellationToken)
    {
        Project project = await context.Projects.SingleAsync(x => x.Id == projectId, cancellationToken);

        if (project.CreatedById == userId)
        {
            return true; // Creator always has access
        }

        return await HasWorkspaceAccess(userId, project.Workspace.Id, cancellationToken);
    }

    public async Task<bool> HasAccessToBoard(Guid userId, Guid boardId, CancellationToken cancellationToken)
    {
        Board board = await context.Boards
            .Include(b => b.Project)
            .ThenInclude(p => p.Workspace)
            .SingleAsync(b => b.Id == boardId, cancellationToken);

        if (board.CreatedById == userId)
        {
            return true; // Creator always has access
        }

        return await HasWorkspaceAccess(userId, board.Project.Workspace.Id, cancellationToken);
    }

    public async Task<bool> HasAccessToTask(Guid userId, Guid taskId, CancellationToken cancellationToken)
    {
        Domain.Tasks.Task task = await context.Tasks
            .Include(t => t.Board)
            .ThenInclude(b => b.Project)
            .ThenInclude(p => p.Workspace)
            .SingleAsync(t => t.Id == taskId, cancellationToken);

        if (task.CreatedById == userId)
        {
            return true; // Creator always has access
        }

        return await HasWorkspaceAccess(userId, task.Board.Project.Workspace.Id, cancellationToken);
    }
}
