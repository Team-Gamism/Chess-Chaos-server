using Server.Service.Interface;
using StackExchange.Redis;

namespace Server.Service;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IDatabase _redisDb;
    private const string RefreshTokenKey = "refresh:";

    public RefreshTokenService(IDatabase redisDb)
    {
        _redisDb = redisDb;
    }
    
    public async Task SaveRefreshTokenAsync(string playerId, string refreshToken, DateTime expiryDate)
    {
        var key = RefreshTokenKey + playerId;
        var ttl = expiryDate - DateTime.UtcNow;
        
        await _redisDb.StringSetAsync(key, refreshToken, ttl);
    }

    public async Task<bool> ValidateRefreshTokenAsync(string playerId, string refreshToken)
    {
        var key = RefreshTokenKey + playerId;
        var storedToken = await _redisDb.StringGetAsync(key);
        
        return storedToken == refreshToken;
    }

    public async Task ReplaceRefreshTokenAsync(string playerId, string oldToken, string newToken, DateTime newExpiry)
    {
        var key = RefreshTokenKey + playerId;
        var currentToken = await _redisDb.StringGetAsync(key);

        if (currentToken == oldToken)
        {
            var ttl = newExpiry - DateTime.UtcNow;
            await _redisDb.StringSetAsync(key, newToken, ttl);
        }
    }

    public async Task DeleteRefreshTokenAsync(string playerId)
    {
        var key = RefreshTokenKey + playerId;
        await _redisDb.KeyDeleteAsync(key);
    }
}