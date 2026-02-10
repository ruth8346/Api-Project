using Microsoft.AspNetCore.Mvc;
using TrickyTrayAPI.Models;

namespace TrickyTrayAPI.Controllers
{
    [ApiController]
    [Route("api/system-state")]
    public class SystemStateController : ControllerBase
    {
        private readonly SystemStateService _service;

        public SystemStateController(SystemStateService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<SystemState> GetState() => Ok(_service.GetState());

        [HttpPost("start")]
        public IActionResult StartSale()
        {
            try
            {
                _service.StartSale();
                return Ok(new { message = "Sale started successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("finish")]
        public async Task<IActionResult> FinishSale()
        {
            try
            {
                // קריאה לפונקציה האסינכרונית החדשה
                var winners = await _service.FinishSaleAsync();
                return Ok(new
                {
                    message = "Sale finished and winners drawn successfully.",
                    winners = winners
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("reset")]
        public IActionResult Reset()
        {
            _service.Reset();
            return Ok(new { message = "System reset successfully." });
        }
    }
}