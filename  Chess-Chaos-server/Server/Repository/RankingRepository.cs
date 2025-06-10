using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Model.Ranking.Entity;
using Server.Repository.Interface;

namespace Server.Repository;

public class RankingRepository : IRankingRepository
{
    private readonly AppDbContext _dbContext;

    public RankingRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<PlayerRankingData?> GetByPlayerIdAsync(string playerId)
    {
        return await _dbContext.PlayerRankings
            .SingleOrDefaultAsync(p => p.PlayerId == playerId);
    }

    public async Task<List<PlayerRankingData>> GetTopAsync(int count)
    {
        return await _dbContext.PlayerRankings
            .OrderByDescending(p => p.Score)
            .Take(count)
            .ToListAsync();
    }

    public async Task AddAsync(PlayerRankingData data)
    {
        await _dbContext.PlayerRankings.AddAsync(data);
    }

    public async Task UpdateAsync(PlayerRankingData data)
    {
        _dbContext.PlayerRankings.Update(data);
        await Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}