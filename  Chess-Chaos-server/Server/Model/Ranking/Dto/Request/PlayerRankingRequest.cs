namespace Server.Model.Ranking.Dto.Request;

public class PlayerRankingRequest
{
    public string PlayerId { get; set; } = null!;
    public int Score { get; set; }
}