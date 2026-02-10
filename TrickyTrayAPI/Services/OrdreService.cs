using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;

namespace TrickyTrayAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly SystemStateService _systemStateService;

        public OrderService(IOrderRepository orderRepository, SystemStateService systemStateService)
        {
            _orderRepository = orderRepository;
            _systemStateService = systemStateService;
        }

        // ✅ חדש: מחזיר Draft פעיל אם יש, ואם אין יוצר חדש
        public async Task<Order> GetOrCreateDraftAsync(int buyerId)
        {
            var draft = await _orderRepository.GetActiveDraftAsync(buyerId);
            if (draft != null) return draft;

            return await _orderRepository.CreateDraftAsync(buyerId);
        }

        // ✅ חדש: מציג תמיד את הסל הפעיל (Draft) של המשתמש
        public async Task<OrderViewDto?> GetActiveOrderViewAsync(int buyerId)
        {
            var draft = await GetOrCreateDraftAsync(buyerId);
            return await GetOrderViewAsync(draft.Id);
        }

        // שליפת סל מלא לפי orderId (נשאר כמו שהיה)
        public async Task<OrderViewDto?> GetOrderViewAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithItemsAsync(orderId);
            if (order == null) return null;

            return new OrderViewDto
            {
                Id = order.Id,
                BuyerId = order.BuyerId,
                BuyerName = order.Buyer.Name,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),

                Items = order.OrderGifts.Select(og => new CartItemDto
                {
                    GiftId = og.GiftId,
                    GiftName = og.Gift.Name,
                    Price = og.Gift.Price,
                    Category = og.Gift.Category
                }).ToList(),

                TotalPrice = order.OrderGifts.Sum(og => og.Gift.Price)
            };
        }

        // ✅ שודרג: במקום ליצור תמיד טיוטה חדשה → GetOrCreate
        // עדיין חתימה זהה, כדי שלא ישבר שום דבר שקרא למתודה הזו
        public Task<Order> CreateDraftAsync(int buyerId)
        {
            return GetOrCreateDraftAsync(buyerId);
        }

        // אישור הזמנה (נשאר כמו שהיה אצלך, כדי לא לשבור מצב-מערכת)
        public async Task<bool> ConfirmOrderAsync(int orderId)
        {
            var systemState = _systemStateService.GetState();
            if (systemState.Status != SystemState.SaleStatus.Draft)
            {
                throw new InvalidOperationException("Cannot finish your order now.");
            }

            return await _orderRepository.ConfirmOrderAsync(orderId);
        }
    }
}
