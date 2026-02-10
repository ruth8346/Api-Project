using TrickyTrayAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrickyTrayAPI.Repositories
{
    public interface IOrderGiftRepository
    {
        Task<bool> AddGiftAsync(int orderId, int giftId);
        Task<bool> RemoveGiftAsync(int orderId, int giftId);
        Task<List<OrderGift>> GetOrdersForGiftAsync(int giftId);
        Task SaveChangesAsync();
        Task <List<OrderGift>> GetAllWinnersAsync();
        Task<List<OrderGift>> GetSortedPurchasesAsync(string sortBy);


    }
}