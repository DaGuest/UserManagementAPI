using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

public class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenAuthenticationMiddleware> _logger;
    private const string AUTH_HEADER = "Authorization";
    private const string BEARER_PREFIX = "Bearer ";
    private const string SECRET_KEY = "YourSuperSecretKey123!YourSuperSecretKey123!"; // Replace with a secure key in production

    public TokenAuthenticationMiddleware(RequestDelegate next, ILogger<TokenAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(AUTH_HEADER, out var authHeader) || !authHeader.ToString().StartsWith(BEARER_PREFIX))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Unauthorized: Missing or invalid token." });
            return;
        }

        var token = authHeader.ToString().Substring(BEARER_PREFIX.Length);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(SECRET_KEY);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Invalid token.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Unauthorized: Invalid token." });
            return;
        }

        await _next(context);
    }
}
