using Server.Model.Token.Dto;

namespace Server.Model.Account.Dto.Response;

public class LoginResponse
{
    public string PlayerId { get; set; } = null!;
    public TokenResponse? Token { get; set; }
    public string Message { get; set; } = String.Empty;
}