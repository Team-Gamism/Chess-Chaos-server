using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Model.Entity;
using Server.Repository.Interface;

namespace Server.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _dbContext;

    public AccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsAsync(string playerId)
    {
        return await _dbContext.Accounts.AsNoTracking().AnyAsync(p => p.PlayerId == playerId);
    }

    public async Task AddAccountAsync(PlayerLoginData account)
    {
        await _dbContext.Accounts.AddAsync(account);
    }

    public async Task<PlayerLoginData?> FindByPlayerIdAsync(string playerId)
    {
        return await _dbContext.Accounts.AsNoTracking().SingleOrDefaultAsync(p => p.PlayerId == playerId);
    }

    public async Task SaveChagesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}