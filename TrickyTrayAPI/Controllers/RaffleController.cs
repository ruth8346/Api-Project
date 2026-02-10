using Microsoft.AspNetCore.Mvc;
using TrickyTrayAPI.Services;

namespace TrickyTrayAPI.Controllers
{
    [ApiController]
    [Route("api/raffle")]
    public class RaffleController : ControllerBase
    {
        private readonly RaffleService _raffleService;

        public RaffleController(RaffleService raffleService)
        {
            _raffleService = raffleService;
        }

        // הגרלה למתנה
        [HttpPost("{giftId}")]
        public async Task<IActionResult> Raffle(int giftId)
        {
            var winner = await _raffleService.RaffleAsync(giftId);

            if (winner == null)
                return BadRequest("אין משתתפים למתנה זו");

            return Ok(new
            {
                GiftId = giftId,
                WinnerId = winner.Id,
                WinnerName = winner.Name
            });
        }

        [HttpGet("winners-report")]
        public async Task<IActionResult> GetWinnersReport()
        {
            var report = await _raffleService.GetWinnersReportAsync();
            return Ok(report);
        }

        [HttpGet("total-revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var revenue = await _raffleService.GetTotalRevenueAsync();
            return Ok(new { totalRevenue = revenue });
        }
    }
}
