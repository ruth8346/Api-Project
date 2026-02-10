using Microsoft.AspNetCore.Mvc;
using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Services;
using static TrickyTrayAPI.DTOs.BuyerDto;

namespace TrickyTrayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuyerController : Controller
    {

        private readonly IBuyerService _buyerService;

        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var buyer = await _buyerService.GetAllAsync();
            return Ok(buyer);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BuyerViewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BuyerViewDto>> GetById(int id)
        {
            var buyer = await _buyerService.GetByIdAsync(id);

            if (buyer == null)
            {
                return NotFound(new { message = $"Buyer with ID {id} not found." });
            }

            return Ok(buyer);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BuyerViewDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BuyerViewDto>> Create([FromBody] BuyerCreateDto createDto)
        {
            try
            {
                var buyer = await _buyerService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = buyer.Id }, buyer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BuyerViewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BuyerViewDto>> UpdateAsync(int id, [FromBody] BuyerUpdateDto updateDto)
        {
            try
            {
                var buyer = await _buyerService.UpdateAsync(id, updateDto);

                if (buyer == null)
                {
                    return NotFound(new { message = $"Buyer with ID {id} not found." });
                }

                return Ok(buyer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BuyerViewDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<BuyerViewDto>> DeleteAsync(int id)
        {

            var deleted = await _buyerService.DeleteAsync(id);

            if (deleted == false)
            {
                return NotFound(new { message = $"Buyer with ID {id} not found." });
            }

            return NoContent();

        }
    }
}
