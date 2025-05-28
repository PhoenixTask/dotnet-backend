using Application.Abstractions.Messaging;

namespace Application.Users.UploadProfile;
public sealed class UploadProfileCommand : ICommand
{
    public string Base64File { get; set; }
    public string FileName { get; set; }
    public Guid UserId { get; set; }
}
