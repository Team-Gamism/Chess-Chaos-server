using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Configuration;
using Server.Service.Interface;

namespace Server.Service;

public class JwtService : IJwtService
{
    private readonly JwtSetting _setting;

    public JwtService(IOptions<JwtSetting> setting)
    {
        _setting = setting.Value;
    }

    public string GenerateToken(string playerId)
    {
        var claim = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, playerId),
            new Claim(ClaimTypes.Name, playerId),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _setting.Issuer,
            audience: _setting.Audience,
            claims: claim,
            expires: DateTime.UtcNow.AddMinutes(_setting.ExpiryMinutes),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}