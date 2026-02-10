using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Services;

namespace TrickyTrayAPI.Controllers
{

        [ApiController]
        [Route("api/[controller]")]
        public class DonorController : Controller
        {

        private readonly IDonorService _donorService;

            public DonorController(IDonorService donorService)
            {
                _donorService = donorService;
            }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var donors = await _donorService.GetAllDonorsAsync();
            return Ok(donors);
        }

        [HttpGet("{id}")]
            [ProducesResponseType(typeof(DonorViewDto), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<DonorViewDto>> GetById(int id)
            {
                var donor = await _donorService.GetByIdAsync(id);

                if (donor == null)
                {
                    return NotFound(new { message = $"Donor with ID {id} not found." });
                }

                return Ok(donor);
            }

            [HttpPost]
            [ProducesResponseType(typeof(DonorViewDto), StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [Authorize(Roles = "manager")]
        public async Task<ActionResult<DonorViewDto>> Create([FromBody] DonorCreateDto createDto)
            {
                try
                {
                    var donor = await _donorService.CreateAsync(createDto);
                    return CreatedAtAction(nameof(GetById), new { id = donor.Id }, donor);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            [HttpPut("{id}")]
            [ProducesResponseType(typeof(DonorViewDto), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [Authorize(Roles = "manager")]
        public async Task<ActionResult<DonorViewDto>> UpdateAsync(int id, [FromBody] DonorUpdateDto updateDto)
            {
                try
                {
                    var donor = await _donorService.UpdateAsync(id, updateDto);

                    if (donor == null)
                    {
                        return NotFound(new { message = $"Donor with ID {id} not found." });
                    }

                    return Ok(donor);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpDelete("{id}")]
            [ProducesResponseType(typeof(DonorViewDto), StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [Authorize(Roles = "manager")]

        public async Task<ActionResult<DonorViewDto>> DeleteAsync(int id)
            {
                var deleted = await _donorService.DeleteAsync(id);

                if (deleted == false)
                {
                    return NotFound(new { message = $"Donor with ID {id} not found." });
                }
                return NoContent();
            }

        // חיפוש תורמים לפי קריטריונים שונים
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] string? email,
            [FromQuery] string? gift)
                {
                    var result = await _donorService.SearchDonorsAsync(name, email, gift);
                    return Ok(result);
                }

            }
}
