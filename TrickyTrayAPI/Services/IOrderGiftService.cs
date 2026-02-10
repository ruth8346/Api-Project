using System.Collections.Generic;
using System.Threading.Tasks;
using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;

namespace TrickyTrayAPI.Services
{
    public interface IOrderGiftService
    {
        Task<bool> AddGiftAsync(int orderId, int giftId);
        Task<bool> RemoveGiftAsync(int orderId, int giftId);
        Task<List<OrderGift>> GetOrdersForGiftAsync(int giftId);
        Task<List<OrderGiftDto>> GetSortedPurchasesAsync(string sortBy);
    }
}