namespace Web.Api.Infrastructure;

internal static class CookieService
{
    internal static void SetToken(string access, string refresh, HttpContext httpContext)
    {
        httpContext.Response.Cookies.Append("access", access, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(60),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
        httpContext.Response.Cookies.Append("refresh", refresh, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
    }
    internal static string GetRefreshToken(HttpContext httpContext)
    {
        httpContext.Request.Cookies.TryGetValue("refresh", out string? refresh);
        return refresh;
    }

    internal static void ExpireToken(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete("access");
        httpContext.Response.Cookies.Delete("refresh");
    }
}
