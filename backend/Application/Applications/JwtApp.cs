using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Config;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public class JwtApp : IJwtApp
{
    private readonly JwtSettings _jwtSettings;

    public JwtApp(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        if (user == null)
            throw new ArgumentException("Usuário não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(_jwtSettings.Secret))
            throw new InvalidOperationException("JwtSettings.Secret não foi configurado.");

        var claims = new List<Claim>
        {
            new("usuarioId", user.ID.ToString()),
            new("nome", user.Name),
            new("email", user.Email)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}