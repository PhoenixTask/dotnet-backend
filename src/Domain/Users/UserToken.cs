namespace Domain.Users;
public sealed class UserToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public TokenType TokenType { get; set; }
    public DateTime ExpireOnUtc { get; set; }

    public User User { get; set; }
}
public enum TokenType
{
    RefreshToken = 0,
    ForgetPasswordToken = 1
}
