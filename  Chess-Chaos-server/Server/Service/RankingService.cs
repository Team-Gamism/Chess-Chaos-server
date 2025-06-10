using Server.Model.Ranking.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class RankingService : IRankingService
{
    private readonly IRankingRepository _repository;

    public RankingService(IRankingRepository repository)
    {
        _repository = repository;
    }
    
    public async Task SubmitRankingAsync(string playerId, int score)
    {
        var exists = await _repository.GetByPlayerIdAsync(playerId);

        if (exists == null)
        {
            var newRanking = new PlayerRankingData
            {
                PlayerId = playerId,
                Score = score,
            };
            
            await _repository.AddAsync(newRanking);
        }
        else if (score > exists.Score)
        {
            exists.Score = score;
            await _repository.UpdateAsync(exists);
        }
        
        await _repository.SaveAsync();
    }

    public async Task<List<PlayerRankingData>> GetTopRankingsAsync(int count)
    {
        return await _repository.GetTopAsync(count);
    }

    public async Task<PlayerRankingData?> GetPlayerRankingAsync(string playerId)
    {
        return await _repository.GetByPlayerIdAsync(playerId);
    }
}