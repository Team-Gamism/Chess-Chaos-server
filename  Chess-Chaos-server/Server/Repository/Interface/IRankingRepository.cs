using Server.Model.Ranking.Entity;

namespace Server.Repository.Interface;

public interface IRankingRepository
{
    Task<PlayerRankingData?> GetByPlayerIdAsync(string playerId);
    Task<List<PlayerRankingData>> GetTopAsync(int count);
    Task AddAsync(PlayerRankingData data);
    Task UpdateAsync(PlayerRankingData data);
    Task SaveAsync();
}