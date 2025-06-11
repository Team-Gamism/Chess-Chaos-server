using System.ComponentModel.DataAnnotations;

namespace Server.Model.Account.Dto.Request;

public class RegisterRequest
{
    [Required]
    public string PlayerId { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
}