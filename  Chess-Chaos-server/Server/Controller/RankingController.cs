using Microsoft.AspNetCore.Mvc;
using Server.Model.Ranking.Dto.Request;
using Server.Model.Ranking.Dto.Response;
using Server.Service.Interface;

namespace Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        private readonly IRankingService _rankingService;

        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        [HttpPost("submit")]
        public async Task<ActionResult<PlayerRankingResponse>> SubmitPlayerRanking([FromBody] PlayerRankingRequest req)
        {
            await _rankingService.SubmitRankingAsync(req.PlayerId, req.Score);

            var updatedRanking = await _rankingService.GetPlayerRankingAsync(req.PlayerId);
            if (updatedRanking == null)
                return NotFound();
            
            return Ok(new PlayerRankingResponse
            {
                PlayerId = req.PlayerId,
                Score = updatedRanking.Score
            });
        }

        [HttpGet("top/{count}")]
        public async Task<ActionResult<List<PlayerRankingResponse>>> GetTopRankings(int count)
        {
            var rankings = await _rankingService.GetTopRankingsAsync(count);

            var res = rankings
                .Select(r => new PlayerRankingResponse
                {
                    PlayerId = r.PlayerId,
                    Score = r.Score
                })
                .ToList();
            
            return Ok(res);
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<PlayerRankingResponse>> GetRankingByPlayerId(string playerId)
        {
            var ranking = await _rankingService.GetPlayerRankingAsync(playerId);

            if (ranking == null)
                return NotFound();

            return Ok(new PlayerRankingResponse
            {
                PlayerId = ranking.PlayerId,
                Score = ranking.Score
            });
        }
    }
}
