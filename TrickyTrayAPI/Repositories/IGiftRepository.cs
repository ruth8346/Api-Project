using TrickyTrayAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrickyTrayAPI.Repositories
{
    public interface IGiftRepository
    {
        Task<IEnumerable<Gift>> GetAllAsync();
        Task<Gift?> GetByIdAsync(int id);
        Task<Gift> CreateAsync(Gift gift);
        Task<Gift?> UpdateAsync(Gift gift);
        Task<bool> DeleteAsync(int id);
        Task<List<Gift>> SearchGiftsAsync(string? name, string? donorName, int? buyersCount);
        Task<List<Gift>> GetSortedGiftsAsync(string sortBy);

    }
}