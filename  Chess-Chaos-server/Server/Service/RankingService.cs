using Server.Model.Ranking.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class RankingService : IRankingService
{
    private readonly IRankingRepository _repository;
    private readonly ICacheService _cacheService;

    private const string TopRankingCachekey = "Ranking:Top";
    
    public RankingService(IRankingRepository repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
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
        string cacheKey =  $"{TopRankingCachekey}:{count}";

        var cachedData = await _cacheService.GetDataAsync<List<PlayerRankingData>>(cacheKey);
        if (cachedData != null)
            return cachedData;
        
        var rankings = await _repository.GetTopAsync(count);
        
        await _cacheService.SetDataAsync(cacheKey, rankings, TimeSpan.FromMinutes(5));

        return rankings;
    }

    public async Task<PlayerRankingData?> GetPlayerRankingAsync(string playerId)
    {
        return await _repository.GetByPlayerIdAsync(playerId);
    }
}