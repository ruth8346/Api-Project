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

        // 📌 השגת הסטטוס הנוכחי
        [HttpGet]
        public ActionResult<SystemState> GetState()
        {
            var state = _service.GetState();
            return Ok(state);
        }

        // 📌 פתיחת המכירה (Start)
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

        // 📌 סיום המכירה והגרלה (Finish)
        [HttpPost("finish")]
        public IActionResult FinishSale()
        {
            try
            {
                _service.FinishSale();
                return Ok(new { message = "Sale finished successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 📌 איפוס המערכת (Reset)
        [HttpPost("reset")]
        public IActionResult Reset()
        {
            _service.Reset();
            return Ok(new { message = "System reset successfully." });
        }
    }
}