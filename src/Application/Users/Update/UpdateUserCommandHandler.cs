using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
namespace Application.Users.Update;

internal sealed class UpdateUserCommandHandler(
    IApplicationDbContext context, IUserContext userContext) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;
        User? user = await context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(userId));
        }
        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            user.FirstName = request.FirstName;
        }
        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            user.LastName = request.LastName;
        }
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
