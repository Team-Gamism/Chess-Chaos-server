using System.Diagnostics;
using Server.Model.Account.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class AuthService : IAuthService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;
    
    public AuthService(IAccountRepository accountRepository,
        IPasswordService passwordService, IJwtService jwtService,
        ILogger<AuthService> logger)
    {
        _accountRepository = accountRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _logger = logger;
    }
    
    public async Task<bool> RegisterAsync(string playerId, string password)
    {
        var stopwatch = Stopwatch.StartNew();
        
        bool exists = await _accountRepository.ExistsAsync(playerId);
        if (exists)
        {
            _logger.LogInformation("RegisterAsync failed: PlayerId '{PlayerId}' already exists", playerId);
            return false;
        }
        
        var hashedPassword = _passwordService.HashPassword(password);

        await _accountRepository.AddAccountAsync(new PlayerLoginData
        {
            PlayerId = playerId,
            Password = hashedPassword
        });
        
        stopwatch.Stop();
        _logger.LogInformation("RegisterAsync succeeded for '{PlayerId}' in {ElapsedMilliseconds} ms",
            playerId, stopwatch.ElapsedMilliseconds);
        
        return true;
    }
    
    public async Task<string?> LoginAsync(string playerId, string password)
    {
        var stopwatch = Stopwatch.StartNew();

        var account = await _accountRepository.FindByPlayerIdAsync(playerId);
        if (account == null)
        {
            _logger.LogWarning("LoginAsync failed: PlayerId '{PlayerId}' not found", playerId);
            return null;
        }

        if (!_passwordService.VerifyPassword(password, account.Password))
        {
            _logger.LogWarning("LoginAsync failed: Incorrect password for PlayerId '{PlayerId}'", playerId);
            return null;
        }

        var token = _jwtService.GenerateToken(playerId);

        stopwatch.Stop();
        _logger.LogInformation("LoginAsync succeeded for '{PlayerId}' in {ElapsedMilliseconds} ms",
            playerId, stopwatch.ElapsedMilliseconds);

        return token;
    }
}