using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model.Account.Entity;

[Table("player_login_data")]
public class PlayerLoginData
{
    [Key]
    [Column("player_id")]
    [MaxLength(15)]
    public string PlayerId { get; set; } = null!;
    
    [Column("password")]
    [MaxLength(60)]
    public string Password { get; set; } = null!;
}