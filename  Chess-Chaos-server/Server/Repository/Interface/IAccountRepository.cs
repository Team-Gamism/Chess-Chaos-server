using Server.Model.Account.Entity;

namespace Server.Repository.Interface;

public interface IAccountRepository
{
    Task<bool> ExistsAsync(string playerId);
    Task AddAccountAsync(PlayerLoginData account);
    Task<PlayerLoginData?> FindByPlayerIdAsync(string playerId);
    Task SaveChagesAsync();
}