using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model.Ranking.Dto.Request;

[Table("player_ranking_data")]
public class PlayerRankingRequest
{
    [Column("player_id")]
    public string PlayerId { get; set; } = null!;
    
    [Column("score")]
    public int Score { get; set; }
}