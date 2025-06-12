namespace Server.Service.Interface;

public interface IJwtService
{
    string GenerateToken(string playerId);
    public string GenerateRefreshToken();
}