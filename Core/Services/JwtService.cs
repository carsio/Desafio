using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Core.Models;
using Core.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Core.Services;

public class JwtService : IJwtService {
    private readonly IConfiguration _configuration;
    private readonly string? _secretKey;
    private readonly string? _issuer;
    private readonly string? _audience;

    public JwtService(IConfiguration configuration) {
        _configuration = configuration;
        _secretKey = _configuration["Jwt:Key"];
        _issuer = _configuration["Jwt:Issuer"];
        _audience = _configuration["Jwt:Audience"];
    }

    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(_issuer, _audience, claims, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public User? ValidateToken(string token) {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenHandler = new JwtSecurityTokenHandler();

        try {
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = credentials.Key
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var user = new User {
                Email = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value
            };

            return user;
        } catch {
            Console.WriteLine("Invalid token");
            return null;
        }
    }
}