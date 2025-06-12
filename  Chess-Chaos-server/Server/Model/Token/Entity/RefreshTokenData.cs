using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model.Token.Entity;

[Table("refresh_token_data")]
public class RefreshTokenData
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;
    
    [Required]
    [Column("player_id")]
    [MaxLength(15)]
    public string PlayerId { get; set; } = null!;
    
    [Required]
    [Column("refresh_token")]
    [MaxLength(255)]
    public string RefreshToken { get; set; } = null!;
    
    [Required]
    [Column("expiry_date")]
    public DateTime ExpiryDate { get; set; }
}