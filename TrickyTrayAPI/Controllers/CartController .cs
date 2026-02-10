using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrickyTrayAPI.Services;

namespace TrickyTrayAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderGiftService _orderGiftService;

        public CartController(IOrderService orderService, IOrderGiftService orderGiftService)
        {
            _orderService = orderService;
            _orderGiftService = orderGiftService;
        }
        
        [HttpGet("active-id")]
        public async Task<IActionResult> GetActiveId([FromQuery] int buyerId)
        {
            if (buyerId <= 0)
                return BadRequest(new { message = "buyerId is required", buyerId });

            var draft = await _orderService.GetOrCreateDraftAsync(buyerId);
            return Ok(new { id = draft.Id });
        }

        // שליפת סל
        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetCart(int orderId)
        {
            var cart = await _orderService.GetOrderViewAsync(orderId);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        // הוספת מתנה לסל
        [HttpPost("{orderId}/add/{giftId}")]
        public async Task<IActionResult> AddGift(int orderId, int giftId)
        {
            var success = await _orderGiftService.AddGiftAsync(orderId, giftId);
            if (!success)
                return BadRequest("Cannot add gift.");

            return Ok();
        }

        // הסרת מתנה מהסל
        [HttpDelete("{orderId}/remove/{giftId}")]
        public async Task<IActionResult> RemoveGift(int orderId, int giftId)
        {
            var success = await _orderGiftService.RemoveGiftAsync(orderId, giftId);
            if (!success)
                return BadRequest("Cannot remove gift.");

            return Ok();
        }

        // אישור הזמנה
        [HttpPost("{orderId}/confirm")]
        public async Task<IActionResult> Confirm(int orderId)
        {
            var success = await _orderService.ConfirmOrderAsync(orderId);
            if (!success)
                return BadRequest("Cannot confirm order.");

            return Ok();
        }

    }
}
