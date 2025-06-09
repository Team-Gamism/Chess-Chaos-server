using Server.Model.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class AuthService : IAuthService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordService _passwordService;

    public AuthService(IAccountRepository accountRepository, IPasswordService passwordService)
    {
        _accountRepository = accountRepository;
        _passwordService = passwordService;
    }
    
    public async Task<bool> RegisterAsync(string playerId, string password)
    {
        bool exists = await _accountRepository.ExistsAsync(playerId);
        if (!exists) 
            return false;
        
        var hashedPassword = _passwordService.HashPassword(password);

        await _accountRepository.AddAccountAsync(new PlayerLoginData
        {
            PlayerId = playerId,
            Password = hashedPassword
        });
        
        await _accountRepository.SaveChagesAsync();
        return true;
    }
    
    public async Task<string?> LoginAsync(string playerId, string password)
    {
        var user = await _accountRepository.FindByPlayerIdAsync(playerId);
        if (user == null)
            return null;
        
        if (!_passwordService.VerifyPassword(password, user.Password))
            return null;
        
        return Guid.NewGuid().ToString(); // 나중에 JWT 토큰 반환으로 수정
    }
}