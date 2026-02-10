using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;
using static TrickyTrayAPI.DTOs.BuyerDto;

namespace TrickyTrayAPI.Services
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepository _giftRepo;
        private readonly SystemStateService _systemStateService;
        private readonly IOrderGiftRepository _orderGiftRepository;

        public GiftService(IGiftRepository giftRepo, SystemStateService systemStateService, IOrderGiftRepository orderGiftRepository)
        {
            _giftRepo = giftRepo;
            _systemStateService = systemStateService;
            _orderGiftRepository = orderGiftRepository;
        }

        public async Task<IEnumerable<GiftViewDto>> GetAllAsync()
        {
            var gifts = await _giftRepo.GetAllAsync();

            return gifts.Select(b => MapToViewDto(b));
        }

        public async Task<GiftViewDto?> GetByIdAsync(int id)
        {
            var gift = await _giftRepo.GetByIdAsync(id);

            return gift != null ? MapToViewDto(gift) : null;
        }
        private GiftViewDto MapToViewDto(Gift g)
        {
            return new GiftViewDto
            {
                Id = g.Id,
                Name = g.Name,
                Price = g.Price,
                ImageUrl = g.ImageUrl,
                DonorName = g.Donor?.Name ?? "תורם אנונימי"
            };
        }

        public async Task<GiftViewDto> CreateAsync(GiftCreateDto createDto)
        {
            var systemState = _systemStateService.GetState();
            if (systemState.Status != SystemState.SaleStatus.Draft)
            {
                throw new InvalidOperationException("Cannot create gift after the sale has started.");
            }

            var gift = new Gift
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                ImageUrl = createDto.ImageUrl,
                Category = createDto.Category,
                DonorId = createDto.DonorId
            };


            var savedGift = await _giftRepo.CreateAsync(gift);


            return MapToViewDto(savedGift);
        }

        public async Task<GiftViewDto?> UpdateAsync(int id, GiftUpdateDto updateDto)
        {
            var systemState = _systemStateService.GetState();
            if (systemState.Status != SystemState.SaleStatus.Draft)
            {
                throw new InvalidOperationException("Cannot update gift after the sale has started.");
            }
            var existingGift = await _giftRepo.GetByIdAsync(id);
            if (existingGift == null) return null;

            if (updateDto.Name != null) existingGift.Name = updateDto.Name;
            if (updateDto.Description != null) existingGift.Description = updateDto.Description;
            if (updateDto.Price != 0) existingGift.Price = updateDto.Price;
            if (updateDto.DonorId.HasValue) existingGift.DonorId = updateDto.DonorId.Value;
            if (updateDto.Category != null) existingGift.Category = updateDto.Category;
            if (updateDto.ImageUrl != null) existingGift.ImageUrl = updateDto.ImageUrl;


            var update = await _giftRepo.UpdateAsync(existingGift);
            return update != null ? MapToViewDto(update) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var state = _systemStateService.GetState();

            // אם המכירה הסתיימה – אין מחיקות
            if (state.Status == SystemState.SaleStatus.Finished)
                throw new InvalidOperationException("Cannot delete gifts after sale is finished");

            // אם המכירה פעילה – מותר למחוק רק אם אין רכישות
            if (state.Status == SystemState.SaleStatus.Active)
            {
                var orders = await _orderGiftRepository.GetOrdersForGiftAsync(id);
                if (orders.Any())
                    throw new InvalidOperationException("Cannot delete gift that already has purchases");
            }

            // Draft או Active בלי רכישות
            return await _giftRepo.DeleteAsync(id);
        }

        public async Task<List<GiftViewDto>> SearchGiftsAsync(string? name, string? donorName, int? buyersCount)
        {
            var gifts = await _giftRepo.SearchGiftsAsync(name, donorName, buyersCount);

            return gifts.Select(g => new GiftViewDto
            {
                Id = g.Id,
                Name = g.Name,
                DonorName = g.Donor?.Name ?? "תורם אנונימי",
                ImageUrl = g.ImageUrl
            }).ToList();
        }

        public async Task<List<GiftViewDto>> GetSortedGiftsAsync(string sortBy)
        {
            var gifts = await _giftRepo.GetSortedGiftsAsync(sortBy);

            return gifts.Select(g => new GiftViewDto
            {
                Id = g.Id,
                Name = g.Name,
                Price = g.Price,
                DonorName = g.Donor?.Name ?? "תורם אנונימי",
                ImageUrl = g.ImageUrl,
                Category = g.Category
            }).ToList();
        }


    }
}