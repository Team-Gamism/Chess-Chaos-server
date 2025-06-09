using System.ComponentModel.DataAnnotations;

namespace Server.Model.Dto.Request;

public class LoginRequest
{
    [Required]
    public string PlayerId { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
}