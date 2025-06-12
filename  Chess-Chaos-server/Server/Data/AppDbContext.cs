using Microsoft.EntityFrameworkCore;
using Server.Model.Ranking.Entity;
using Server.Model.Token.Entity;

namespace Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<PlayerRankingData> PlayerRankings { get; set; } = null!;
    public DbSet<RefreshTokenData> RefreshTokens { get; set; } = null!;
}