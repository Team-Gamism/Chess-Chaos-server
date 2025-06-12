using Server.Model.Ranking.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class RankingService : IRankingService
{
    private readonly IRankingRepository _repository;
    private readonly ICacheService _cacheService;
    private ILogger<RankingService> _logger;

    private const string TopRankingCachekey = "Ranking:Top";
    
    public RankingService(IRankingRepository repository, ICacheService cacheService, ILogger<RankingService> logger)
    {
        _repository = repository;
        _cacheService = cacheService;
        _logger = logger;
    }
    
    public async Task SubmitRankingAsync(string playerId, int score)
    {
        var exists = await _repository.GetByPlayerIdAsync(playerId);

        bool updated = false;
        
        if (exists == null)
        {
            _logger.LogInformation($"랭킹 등록, playerId: {playerId}, score: {score}");
            var newRanking = new PlayerRankingData
            {
                PlayerId = playerId,
                Score = score,
            };
            
            await _repository.AddAsync(newRanking);
            updated = true;
        }
        else if (score > exists.Score)
        {
            _logger.LogInformation($"플레이어 {playerId} 점수 갱신: {score}");
            exists.Score = score;
            await _repository.UpdateAsync(exists);
            updated = true;
        }

        if (updated)
        {
            for (int i = 1; i < 6; i++)
            {
                string cacheKey = $"{TopRankingCachekey}:{i}";
                await _cacheService.RemoveDataAsync(cacheKey);
            }
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