using Microsoft.EntityFrameworkCore;
using TrickyTrayAPI.Data;
using TrickyTrayAPI.Models;

namespace TrickyTrayAPI.Repositories
{
    public class OrderGiftRepository : IOrderGiftRepository
    {
        private readonly TrickyTrayDbContext _context;

        public OrderGiftRepository(TrickyTrayDbContext context)
        {
            _context = context;
        }

        // הוספת מתנה לסל
        public async Task<bool> AddGiftAsync(int orderId, int giftId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.Status != Order.OrderStatus.Draft)
                return false;

            var giftExists = await _context.Gifts.AnyAsync(g => g.Id == giftId);
            if (!giftExists) return false;

            var item = new OrderGift
            {
                OrderId = orderId,
                GiftId = giftId,
                IsWinner = false
            };

            _context.OrderGift.Add(item);
            await _context.SaveChangesAsync();
            return true;
        }

        // הסרת מתנה מהסל
        public async Task<bool> RemoveGiftAsync(int orderId, int giftId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.Status != Order.OrderStatus.Draft)
                return false;

            var item = await _context.OrderGift
                .FirstOrDefaultAsync(x => x.OrderId == orderId && x.GiftId == giftId);

            if (item == null) return false;

            _context.OrderGift.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        // קבלת כל ההזמנות עבור מתנה מסוימת
        public async Task<List<OrderGift>> GetOrdersForGiftAsync(int giftId)
        {
            return await _context.OrderGift
                .Where(og =>
                    og.GiftId == giftId &&
                    og.Order.Status == Order.OrderStatus.Confirmed)
                .Include(og => og.Order)
                    .ThenInclude(o => o.Buyer)
                .ToListAsync();
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderGift>> GetAllWinnersAsync()
        {
            return await _context.OrderGift
                .Where(og => og.IsWinner)
                .Include(og => og.Gift)
                .Include(og => og.Order)
                    .ThenInclude(o => o.Buyer)
                .ToListAsync();
        }

        public async Task<List<OrderGift>> GetSortedPurchasesAsync(string sortBy)
        {
            var query = _context.OrderGift
                .Include(og => og.Gift)
                .Include(og => og.Order)
                    .ThenInclude(o => o.Buyer)
                .AsQueryable();

            if (sortBy == "price")
            {
                query = query.OrderByDescending(og => og.Gift.Price);
            }
            else if (sortBy == "popularity")
            {
                query = query
                    .OrderByDescending(og =>
                        _context.OrderGift.Count(x => x.GiftId == og.GiftId)
                    );
            }

            return await query.ToListAsync();
        }



    }
}
