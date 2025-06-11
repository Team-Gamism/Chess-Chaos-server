using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Server.Service.Interface;

namespace Server.Service;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task<T?> GetDataAsync<T>(string key)
    {
        var data = await _cache.GetStringAsync(key);
        return data is null ? default : JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetDataAsync<T>(string key, T value, TimeSpan? expiredTime = null)
    {
        var options = new DistributedCacheEntryOptions();
        if (expiredTime.HasValue)
            options.AbsoluteExpirationRelativeToNow = expiredTime.Value;
        
        var data = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, data, options);
    }

    public async Task RemoveDataAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}