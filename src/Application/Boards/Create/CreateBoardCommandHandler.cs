using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.Create;

internal sealed class CreateBoardCommandHandler(
    IApplicationDbContext context, IUserContext userContext) : ICommandHandler<CreateBoardCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;
        Project? project = await context.Projects
            .SingleOrDefaultAsync(x => x.Id == request.ProjectId && x.CreatedById == userId, cancellationToken: cancellationToken);

        if (project is null)
        {
            return Result.Failure<Guid>(ProjectErrors.NotFound(request.ProjectId));
        }

        var board = new Board
        {
            Color = request.Color,
            IsArchive = false,
            Name = request.Name,
            Project = project,
            Order = request.Order,
        };

        context.Boards.Add(board);
        await context.SaveChangesAsync(cancellationToken);

        return board.Id;
    }
}
