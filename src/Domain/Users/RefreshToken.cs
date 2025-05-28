namespace Domain.Users;
public sealed class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpireOnUtc { get; set; }

    public User User { get; set; }
}
