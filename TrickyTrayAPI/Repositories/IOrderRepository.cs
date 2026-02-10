using TrickyTrayAPI.Models;
using System.Threading.Tasks;

namespace TrickyTrayAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateDraftAsync(int buyerId);
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task<bool> ConfirmOrderAsync(int orderId);
        Task<Order?> GetActiveDraftAsync(int buyerId);
        Task UpdateAsync(Order order); 

    }

}