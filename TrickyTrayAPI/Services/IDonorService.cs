using TrickyTrayAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrickyTrayAPI.Services
{
    public interface IDonorService
    {
        Task<IEnumerable<DonorViewDto>> GetAllDonorsAsync();
        Task<DonorViewDto?> GetByIdAsync(int id);
        Task<DonorViewDto> CreateAsync(DonorCreateDto createDto);
        Task<DonorViewDto?> UpdateAsync(int id, DonorUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<List<DonorViewDto>> SearchDonorsAsync(string? name, string? email, string? giftName);

    }
}