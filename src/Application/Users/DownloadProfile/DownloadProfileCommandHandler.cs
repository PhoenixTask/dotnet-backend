using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.DownloadProfile;

internal sealed class DownloadProfileCommandHandler(IApplicationDbContext context) : ICommandHandler<DownloadProfileCommand, string>
{
    public async Task<Result<string>> Handle(DownloadProfileCommand request, CancellationToken cancellationToken)
    {
        User? user = await context.Users.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<string>(UserErrors.NotFound(request.UserId));
        }
        if (string.IsNullOrEmpty(user.ProfileImage))
        {
            return Result.Failure<string>(Error.NullValue);
        }
        if (!File.Exists(user.ProfileImage))
        {
            return Result.Failure<string>(Error.None);
        }
        string filePath = user.ProfileImage;
        return filePath;
    }
}
