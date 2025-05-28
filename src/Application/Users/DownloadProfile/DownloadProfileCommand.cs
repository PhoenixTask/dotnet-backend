using System.Text;
using Application.Abstractions.Messaging;

namespace Application.Users.DownloadProfile;
public sealed record DownloadProfileCommand(Guid UserId) : ICommand<string>;
