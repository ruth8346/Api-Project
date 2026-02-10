using TrickyTrayAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrickyTrayAPI.Repositories
{
    public interface IBuyerRepository
    {
        Task<IEnumerable<Buyer>> GetAllAsync();
        Task<Buyer?> GetByIdAsync(int id);
        Task<Buyer?> GetByEmailAsync(string email);
        Task<Buyer> CreateAsync(Buyer buyer);
        Task<Buyer?> UpdateAsync(Buyer buyer);
        Task<bool> DeleteAsync(int id);
    }
}