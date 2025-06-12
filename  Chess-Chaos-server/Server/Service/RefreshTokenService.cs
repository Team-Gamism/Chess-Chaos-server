using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Model.Token.Entity;
using Server.Service.Interface;

namespace Server.Service;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly AppDbContext _context;

    public RefreshTokenService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task SaveRefreshTokenAsync(string playerId, string refreshToken, DateTime expiryDate)
    {
        var exists = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.PlayerId == playerId);

        if (exists != null)
        {
            exists.RefreshToken = refreshToken;
            exists.ExpiryDate = expiryDate;
        }
        else
        {
            var toeknData = new RefreshTokenData
            {
                PlayerId = playerId,
                RefreshToken = refreshToken,
                ExpiryDate = expiryDate
            };
            
            await _context.RefreshTokens.AddAsync(toeknData);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ValidateRefreshTokenAsync(string playerId, string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.PlayerId == playerId && rt.RefreshToken == refreshToken);
        
        return token != null && token.ExpiryDate > DateTime.UtcNow;
    }

    public async Task ReplaceRefreshTokenAsync(string playerId, string oldToken, string newToken, DateTime newExpiry)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.PlayerId == playerId && rt.RefreshToken == oldToken);

        if (token != null)
        {
            token.RefreshToken = newToken;
            token.ExpiryDate = newExpiry;
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteRefreshTokenAsync(string playerId)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.PlayerId == playerId);

        if (token != null)
        {
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }
}