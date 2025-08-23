using Application.Abstractions.Messaging;

namespace Application.Users.UploadProfile;
public sealed record UploadProfileCommand(string Base64File, string FileName) : ICommand;
