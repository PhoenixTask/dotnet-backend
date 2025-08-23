namespace Application.Users.Login;

public sealed record LoginResponse(Guid UserId, string Token, string RefreshToken);
