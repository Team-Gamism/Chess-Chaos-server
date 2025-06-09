using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Model.Entity;
using Server.Service.Interface;

namespace Server.Service;

public class AuthService : IAuthService
{
    public readonly AppDbContext _db;
    public readonly IPasswordService _passwordService;

    public AuthService(AppDbContext db, IPasswordService passwordService)
    {
        _db = db;
        _passwordService = passwordService;
    }
    
    public async Task<bool> RegisterAsync(string playerId, string password)
    {
        bool exists = await _db.PlayerLoginDatas.AnyAsync(p => p.PlayerId == playerId);
        if (!exists) 
            return false;
        
        var hashedPassword = _passwordService.HashPassword(password);

        _db.PlayerLoginDatas.Add(new PlayerLoginData
        {
            PlayerId = playerId,
            Password = hashedPassword
        });
        
        await _db.SaveChangesAsync();
        return true;
    }
    
    public async Task<string?> LoginAsync(string playerId, string password)
    {
        var user = await _db.PlayerLoginDatas.SingleOrDefaultAsync(p => p.PlayerId == playerId);
        if (user == null)
            return null;
        
        if (!_passwordService.VerifyPassword(password, user.Password))
            return null;
        
        return Guid.NewGuid().ToString(); // 나중에 JWT 토큰 반환으로 수정
    }
}