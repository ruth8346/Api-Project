using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;

namespace TrickyTrayAPI.Services
{
    public class RaffleService
    {
        private readonly IOrderGiftRepository _orderGiftRepository;
        public readonly IGiftRepository _giftRepository;
        public RaffleService(IOrderGiftRepository orderGiftRepository, IGiftRepository giftRepository)
        {
            _orderGiftRepository = orderGiftRepository;
            _giftRepository = giftRepository;
        }

        public async Task<Buyer?> RaffleAsync(int giftId)
        {
            var tickets = await _orderGiftRepository.GetOrdersForGiftAsync(giftId);
            if (tickets == null || tickets.Count == 0)
                return null;

            // אפס זכיות קודמות
            foreach (var t in tickets)
                t.IsWinner = false;

            var random = new Random();
            var winner = tickets[random.Next(tickets.Count)];

            winner.IsWinner = true;

            await _orderGiftRepository.SaveChangesAsync();

            return winner.Order.Buyer;
        }

        public async Task<List<RaffleReportDto>> GetWinnersReportAsync()
        {
            var winners = await _orderGiftRepository.GetAllWinnersAsync();

            return winners.Select(w => new RaffleReportDto
            {
                GiftName = w.Gift.Name,
                WinnerName = w.Order.Buyer.Name,
                WinnerEmail = w.Order.Buyer.Email,
                RaffledAt = w.Order.OrderDate
            }).ToList();
        }

        //סך הכנסות
        public async Task<decimal> GetTotalRevenueAsync()
        {
            var gifts = await _giftRepository.GetAllAsync();
            decimal totalRevenue = 0;

            foreach (var gift in gifts)
            {
                var ordersForGift = await _orderGiftRepository.GetOrdersForGiftAsync(gift.Id);
                var ticketsCount = ordersForGift.Count;
                totalRevenue += ticketsCount * gift.Price;
            }

            return totalRevenue;
        }

    }
}
