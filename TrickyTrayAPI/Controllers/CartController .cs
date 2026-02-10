using Microsoft.AspNetCore.Mvc;
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

        // שליפת סל
        [HttpGet("{orderId}")]
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
            try
            {
                var result = await _orderGiftService.AddGiftAsync(orderId, giftId);
                return Ok(new { success = true });
            }
            catch (InvalidOperationException ex)
            {
                // מחזירים 200 OK כדי שהדפדפן לא יצעק, אבל עם success = false
                return Ok(new { success = false, message = ex.Message });
            }
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
