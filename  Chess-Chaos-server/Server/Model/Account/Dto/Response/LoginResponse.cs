namespace Server.Model.Account.Dto.Response;

public class LoginResponse
{
    public string PlayerId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Message { get; set; } = String.Empty;
}