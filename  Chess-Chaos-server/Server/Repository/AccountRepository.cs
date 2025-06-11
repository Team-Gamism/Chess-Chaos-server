using Dapper;
using Server.Model.Account.Entity;
using Server.Repository.Interface;
using MySqlConnector;

namespace Server.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly string _connectionString;

    public AccountRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    private MySqlConnection CreateConnection() => new(_connectionString);
    
    public async Task<bool> ExistsAsync(string playerId)
    {
        const string sql = "SELECT COUNT(1) FROM player_login_data WHERE PlayerId = @PlayerId";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var count = await connection.ExecuteScalarAsync<int>(sql, new { PlayerId = playerId });
        
        return count > 0;
    }

    public async Task AddAccountAsync(PlayerLoginData account)
    {
        const string sql = "INSERT INTO player_login_data (PlayerId, Password) VALUES (@PlayerId, @Password)";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        await connection.ExecuteAsync(sql, account);
    }

    public async Task<PlayerLoginData?> FindByPlayerIdAsync(string playerId)
    {
        const string sql = "SELECT PlayerId, Password FROM player_login_data WHERE PlayerId = @PlayerId";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        return await connection.QuerySingleOrDefaultAsync<PlayerLoginData>(sql, new { PlayerId = playerId });
    }
}