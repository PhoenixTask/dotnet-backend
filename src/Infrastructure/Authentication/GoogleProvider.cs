using Application.Abstractions.Authentication;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Authentication;
internal class GoogleProvider(IConfiguration configuration) : IGoogleProvider
{
    public async Task<Dictionary<string, string>?> GetClaimsAsync(string accessToken, CancellationToken cancellationToken)
    {
        try
        {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(accessToken,
         new GoogleJsonWebSignature.ValidationSettings
         {
             Audience = [configuration["Google:ClientId"]]
         });
            var result = new Dictionary<string, string>
        {
            { "Scope", payload.Scope },
            { "Prn", payload.Prn },
            { "HostedDomain", payload.HostedDomain },
            { "Email", payload.Email },
            { "EmailVerified", payload.EmailVerified.ToString() },
            { "GivenName", payload.GivenName },
            { "FamilyName", payload.FamilyName },
            { "Picture", payload.Picture },
            { "Locale", payload.Locale }
        };
            return result;
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }
}
