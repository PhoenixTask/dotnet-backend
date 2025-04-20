using Application.Abstractions.Messaging;

namespace Application.Users.GetSetting;

public sealed record GetSettingQuery(string Key) : IQuery<string>;
