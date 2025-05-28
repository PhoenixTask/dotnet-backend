namespace Application.Users.Login;

public sealed class LoginResponse
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
