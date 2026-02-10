using TrickyTrayAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrickyTrayAPI.Repositories
{
    public interface IDonorRepository
    {
        Task<IEnumerable<Donor>> GetAllDonorsWithGiftsAsync();
        Task<Donor?> GetByIdAsync(int id);
        Task<Donor> CreateAsync(Donor donor);
        Task<Donor?> UpdateAsync(Donor donor);
        Task<bool> DeleteAsync(int id);
        Task<List<Donor>> SearchDonorsAsync(string? name, string? email, string? giftName);

    }
}