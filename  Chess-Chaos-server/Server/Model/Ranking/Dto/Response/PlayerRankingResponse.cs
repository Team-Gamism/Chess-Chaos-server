namespace Server.Model.Ranking.Dto.Response;

public class PlayerRankingResponse
{
    public string PlayerId { get; set; } = null!;
    public int Score { get; set; }
}