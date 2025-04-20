using Application.Abstractions.Messaging;

namespace Application.Users.SetSetting;

public sealed record SetSettingCommand(string Key,string Value) : ICommand;
