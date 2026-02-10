using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;

namespace TrickyTrayAPI.Services
{
    public class OrderGiftService : IOrderGiftService
    {
        private readonly IOrderGiftRepository _orderGiftRepository;
        private readonly SystemStateService _systemStateService;

        public OrderGiftService(IOrderGiftRepository orderGiftRepository, SystemStateService systemStateService)
        {
            _orderGiftRepository = orderGiftRepository;
            _systemStateService = systemStateService;
        }

        // הוספת מתנה לסל
        public async Task<bool> AddGiftAsync(int orderId, int giftId)

        {
            var systemState = _systemStateService.GetState();
            if (systemState.Status != SystemState.SaleStatus.Active)
            {
                throw new InvalidOperationException("Cannot add gift to cart now.");
            }
            return await _orderGiftRepository.AddGiftAsync(orderId, giftId);
        }

        // הסרת מתנה מהסל
        public async Task<bool> RemoveGiftAsync(int orderId, int giftId)
        {
            var systemState = _systemStateService.GetState();
            if (systemState.Status != SystemState.SaleStatus.Draft)
            {
                throw new InvalidOperationException("Cannot remove gift from cart now.");
            }
            return await _orderGiftRepository.RemoveGiftAsync(orderId, giftId);
        }

        // שליפת הזמנות של מתנה מסוימת
        public async Task<List<OrderGift>> GetOrdersForGiftAsync(int giftId)
        {
            return await _orderGiftRepository.GetOrdersForGiftAsync(giftId);
        }

        public async Task<List<OrderGiftDto>> GetSortedPurchasesAsync(string sortBy)
        {
            var purchases = await _orderGiftRepository.GetSortedPurchasesAsync(sortBy);

            return purchases.Select(p => new OrderGiftDto
            {
                OrderId = p.OrderId,
                BuyerName = p.Order!.Buyer!.Name,
                OrderDate = p.Order.OrderDate,
                Price = p.Gift.Price
            }).ToList();
        }


    }
}
