using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model.Ranking.Entity;

[Table("player_ranking_data")]
public class PlayerRankingData
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(15)]
    public string PlayerId { get; set; } = null!;

    [Required]
    public int Score { get; set; }
}