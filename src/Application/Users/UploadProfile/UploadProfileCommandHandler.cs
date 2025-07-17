using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.UploadProfile;

internal sealed class UploadProfileCommandHandler(IApplicationDbContext context) : ICommandHandler<UploadProfileCommand>
{
    public async Task<Result> Handle(UploadProfileCommand request, CancellationToken cancellationToken)
    {
        User? user = await context.Users.SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(request.UserId));
        }

        string path = "uploads/user-images";
        await EnsureFolderCreated(path);
        string filePath = Path.Combine(path, $"{user.Id}{Path.GetExtension(request.FileName)}");

        byte[] imageByteArray = Convert.FromBase64String(request.Base64File);

        await File.WriteAllBytesAsync(filePath, imageByteArray, cancellationToken);

        user.ProfileImage = filePath;
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private Task EnsureFolderCreated(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return Task.CompletedTask;
    }
}
