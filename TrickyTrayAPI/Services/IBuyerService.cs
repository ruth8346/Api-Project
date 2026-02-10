using TrickyTrayAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrickyTrayAPI.Services
{
    public interface IBuyerService
    {
        Task<IEnumerable<BuyerDto.BuyerViewDto>> GetAllAsync();
        Task<BuyerDto.BuyerViewDto?> GetByIdAsync(int id);
        Task<BuyerDto.BuyerViewDto> CreateAsync(BuyerDto.BuyerCreateDto createDto);
        Task<BuyerDto.BuyerViewDto?> UpdateAsync(int id, BuyerDto.BuyerUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}