using Application.Abstractions.Messaging;

namespace Application.Users.GetSettings;

public sealed record GetSettingsQuery : IQuery<Dictionary<string,string>>;
