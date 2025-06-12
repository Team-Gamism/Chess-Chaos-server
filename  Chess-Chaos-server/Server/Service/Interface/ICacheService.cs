namespace Server.Service.Interface;

public interface ICacheService
{
    Task<T?> GetDataAsync<T>(string key); 
    Task SetDataAsync<T>(string key, T value, TimeSpan? expiredTime = null);
    Task RemoveDataAsync(string key);
}