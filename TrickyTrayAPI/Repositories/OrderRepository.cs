using Microsoft.EntityFrameworkCore;
using TrickyTrayAPI.Data;
using TrickyTrayAPI.Models;

namespace TrickyTrayAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TrickyTrayDbContext _context;

        public OrderRepository(TrickyTrayDbContext context)
        {
            _context = context;
        }

        // יוצר טיוטה חדשה (נשאר כפי שהיה כדי לא לשבור שימושים קיימים)
        public async Task<Order> CreateDraftAsync(int buyerId)
        {
            var order = new Order
            {
                BuyerId = buyerId,
                Status = Order.OrderStatus.Draft,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        // מחזיר הזמנה עם פריטים (לפי orderId)
        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderGifts)
                    .ThenInclude(og => og.Gift)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        // מאשר הזמנה (רק אם היא Draft)
        public async Task<bool> ConfirmOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            if (order.Status != Order.OrderStatus.Draft)
                return false;

            order.Status = Order.OrderStatus.Confirmed;
            order.OrderDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ חדש אצלך כבר: Draft פעיל לפי buyerId
        public async Task<Order?> GetActiveDraftAsync(int buyerId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.BuyerId == buyerId && o.Status == Order.OrderStatus.Draft);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
