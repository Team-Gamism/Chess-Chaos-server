using Server.Model.Token.Dto;

namespace Server.Service.Interface;

public interface IAuthService
{
    Task<TokenResponse?> LoginAsync(string playerId, string password);
    Task<bool> RegisterAsync(string playerId, string password);
}