namespace Application.Abstractions.Authentication;
public interface IGoogleProvider
{
    Task<Dictionary<string, string>?> GetClaimsAsync(string accessToken, CancellationToken cancellationToken);
}
