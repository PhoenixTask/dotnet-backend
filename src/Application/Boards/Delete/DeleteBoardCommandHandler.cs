using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Boards.Delete;

internal sealed class DeleteBoardCommandHandler(
    IApplicationDbContext context, IUserContext userContext) : ICommandHandler<DeleteBoardCommand>
{
    public async Task<Result> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;

        int countDeleted = await context.Boards
            .Where(x => x.Id == request.boardId && x.CreatedById == userId)
            .ExecuteDeleteAsync(cancellationToken);

        return countDeleted > 0 ? Result.Success() : Result.Failure(BoardErrors.NotFound(request.boardId));
    }
}
