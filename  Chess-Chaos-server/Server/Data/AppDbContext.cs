using Microsoft.EntityFrameworkCore;
using Server.Model.Entity;
using Server.Model.Ranking.Entity;

namespace Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<PlayerLoginData> Accounts { get; set; } = null!;
    public DbSet<PlayerRankingData> PlayerRankings { get; set; } = null!;
}