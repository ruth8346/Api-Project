using TrickyTrayAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrickyTrayAPI.Services
{
    public interface IGiftService
    {
        Task<IEnumerable<GiftViewDto>> GetAllAsync();
        Task<GiftViewDto?> GetByIdAsync(int id);
        Task<GiftViewDto> CreateAsync(GiftCreateDto createDto);
        Task<GiftViewDto?> UpdateAsync(int id, GiftUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<List<GiftViewDto>> SearchGiftsAsync(string? name, string? donorName, int? buyersCount);
        Task<List<GiftViewDto>> GetSortedGiftsAsync(string sortBy);

    }
}