namespace Server.Service.Interface;

public interface IRefreshTokenService
{
    Task SaveRefreshTokenAsync(string playerId, string refreshToken, DateTime expiryDate);
    Task<bool> ValidateRefreshTokenAsync(string playerId, string refreshToken);
    Task ReplaceRefreshTokenAsync(string playerId, string oldToken, string newToken, DateTime newExpiry);
    Task DeleteRefreshTokenAsync(string playerId);
}