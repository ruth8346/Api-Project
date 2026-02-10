using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Services;

namespace TrickyTrayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiftController : Controller
    {

        private readonly IGiftService _giftService;
        private readonly IOrderGiftService _orderGiftService;

        public GiftController(IGiftService giftService, IOrderGiftService orderGiftService)
        {
            _giftService = giftService;
            _orderGiftService = orderGiftService;
        }

        // שליפת כל המתנות
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var gifts = await _giftService.GetAllAsync();
            return Ok(gifts);
        }

        // שליפת מתנה לפי מזהה
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GiftViewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GiftViewDto>> GetById(int id)
        {
            var gift = await _giftService.GetByIdAsync(id);

            if (gift == null)
            {
                return NotFound(new { message = $"Gift with ID {id} not found." });
            }

            return Ok(gift);
        }

        // יצירת מתנה חדשה
        [HttpPost]
        [Authorize]
        [Authorize(Roles = "admin")]

        [ProducesResponseType(typeof(GiftViewDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<GiftViewDto>> Create([FromBody] GiftCreateDto createDto)
        {
            try
            {
                var gift = await _giftService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = gift.Id }, gift);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // עדכון מתנה
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GiftViewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<GiftViewDto>> UpdateAsync(int id, [FromBody] GiftUpdateDto updateDto)
        {
            try
            {
                var gift = await _giftService.UpdateAsync(id, updateDto);

                if (gift == null)
                {
                    return NotFound(new { message = $"Gift with ID {id} not found." });
                }

                return Ok(gift);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // מחיקת מתנה
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(GiftViewDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<GiftViewDto>> DeleteAsync(int id)
        {

            var deleted = await _giftService.DeleteAsync(id);

            if (deleted == false)
            {
                return NotFound(new { message = $"Gift with ID {id} not found." });
            }

            return NoContent();

        }

        // שליפת כל ההזמנות שכוללות מתנה מסוימת
        [HttpGet("gift/{giftId}")]
        public async Task<ActionResult<List<OrderGiftDto>>> GetOrdersForGift(int giftId)
        {
            var orderGifts = await _orderGiftService.GetOrdersForGiftAsync(giftId);

            if (orderGifts == null || orderGifts.Count == 0)
            {
                return NotFound(new { message = "לא נמצאו הזמנות למתנה זו." });
            }

            var result = orderGifts.Select(og => new OrderGiftDto
            {
                OrderId = og.OrderId,
                OrderDate = og.Order.OrderDate,
                BuyerName = og.Order!.Buyer!.Name,
            }).ToList();

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] string? donorName,
            [FromQuery] int? buyersCount)
        {
            var result = await _giftService.SearchGiftsAsync(name, donorName, buyersCount);
            return Ok(result);
        }

        [HttpGet("sort")]
        public async Task<IActionResult> GetSortedGifts([FromQuery] string sortBy)
        {
            var result = await _giftService.GetSortedGiftsAsync(sortBy);
            return Ok(result);
        }


    }
}