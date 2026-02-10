using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using System.Threading.Tasks;

namespace TrickyTrayAPI.Services
{
    public interface IOrderService
    {
        Task<OrderViewDto?> GetOrderViewAsync(int orderId);
        Task<Order> CreateDraftAsync(int buyerId);
        Task<bool> ConfirmOrderAsync(int orderId);
        Task<Order> GetOrCreateDraftAsync(int buyerId);
        Task<OrderViewDto?> GetActiveOrderViewAsync(int buyerId);

    }
}