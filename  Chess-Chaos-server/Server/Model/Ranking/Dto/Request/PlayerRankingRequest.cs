using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model.Ranking.Dto.Request;

[Table("player_ranking_data")]
public class PlayerRankingRequest
{
    public string PlayerId { get; set; } = null!;
    
    public int Score { get; set; }
}