using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model.Ranking.Entity;

[Table("player_ranking_data")]
public class PlayerRankingData
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(15)]
    [Column("player_id")]
    public string PlayerId { get; set; } = null!;

    [Required]
    [Column("score")]
    public int Score { get; set; }
}