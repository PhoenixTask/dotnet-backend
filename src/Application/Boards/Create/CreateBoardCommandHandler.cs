using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Access;
using Domain.Projects;
using Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.Create;

internal sealed class CreateBoardCommandHandler(
    IApplicationDbContext context, IUserAccess userAccess) : ICommandHandler<CreateBoardCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        Project? project = await context.Projects
            .Include(x => x.Workspace)
            .SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken: cancellationToken);

        if (project is null)
        {
            return Result.Failure<Guid>(ProjectErrors.NotFound(request.ProjectId));
        }
        bool hasAccess = await userAccess.IsAuthenticatedAsync(project.Workspace.Id, Role.Owner);
        if (!hasAccess)
        {
            return Result.Failure<Guid>(ProjectErrors.NotFound(request.ProjectId));
        }

        int lastBoardOrder = await context.Boards
            .Where(x => x.ProjectId == request.ProjectId)
            .OrderByDescending(x => x.Order)
            .Select(x => x.Order)
            .FirstOrDefaultAsync(cancellationToken);

        var board = new Board
        {
            Color = request.Color,
            IsArchive = false,
            Name = request.Name,
            Project = project,
            Order = lastBoardOrder + 1,
        };

        context.Boards.Add(board);
        await context.SaveChangesAsync(cancellationToken);

        return board.Id;
    }
}
