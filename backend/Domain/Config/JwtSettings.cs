namespace Domain.Config;

/// <summary>
/// Configuration values for JWT token generation and validation.
/// Bind from appsettings.json section "JwtSettings".
/// </summary>
public class JwtSettings
{
    public string Secret { get; set; }
    public int ExpirationMinutes { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
