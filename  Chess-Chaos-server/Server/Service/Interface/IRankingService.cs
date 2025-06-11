using Server.Model.Ranking.Entity;

namespace Server.Service.Interface;

public interface IRankingService
{
    Task SubmitRankingAsync(string playerId, int score);
    Task<List<PlayerRankingData>> GetTopRankingsAsync(int count);
    Task<PlayerRankingData?> GetPlayerRankingAsync(string playerId);
}