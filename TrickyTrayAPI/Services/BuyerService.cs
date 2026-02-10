using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;
using static TrickyTrayAPI.DTOs.BuyerDto;

namespace TrickyTrayAPI.Services
{
    public class BuyerService : IBuyerService
    {

        private readonly IBuyerRepository _buyerRepo;
        public BuyerService(IBuyerRepository buyerRepo)
        {
            _buyerRepo = buyerRepo;
        }

        public async Task<IEnumerable<BuyerDto.BuyerViewDto>> GetAllAsync()
        {
            var buyers = await _buyerRepo.GetAllAsync();

            return buyers.Select(b => MapToViewDto(b));
        }

        public async Task<BuyerDto.BuyerViewDto?> GetByIdAsync(int id)
        {
            var buyer = await _buyerRepo.GetByIdAsync(id);
            return buyer != null ? MapToViewDto(buyer) : null;
        }
        private BuyerViewDto MapToViewDto(Buyer b)
        {
            return new BuyerViewDto
            {
                Id = b.Id,
                Name = b.Name,
                IdentityNumber = b.IdentityNumber,
                Role = b.Role,
                Email = b.Email,
                Phone = b.Phone
            };
        }

        public async Task<BuyerViewDto> CreateAsync(BuyerCreateDto createDto)
        {
            // 1) נרמול אימייל
            var email = createDto.Email?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            // 2) בדיקת כפילות
            var existing = await _buyerRepo.GetByEmailAsync(email);
            if (existing != null)
                throw new ArgumentException("האימייל כבר קיים במערכת");

            var buyer = new Buyer
            {
                Name = createDto.Name,
                IdentityNumber = createDto.IdentityNumber,
                Role = createDto.Role,
                Email = email,
                Phone = createDto.Phone,
                Password = createDto.Password // (אפשר גם Trim אם רוצים)
            };

            var savedBuyer = await _buyerRepo.CreateAsync(buyer);
            return MapToViewDto(savedBuyer);
        }


        public async Task<BuyerDto.BuyerViewDto?> UpdateAsync(int id, BuyerDto.BuyerUpdateDto updateDto)
        {

            var existingBuyer = await _buyerRepo.GetByIdAsync(id);
            if (existingBuyer == null) return null;

            if (updateDto.Name != null) existingBuyer.Name = updateDto.Name;
            if (updateDto.Email != null) existingBuyer.Email = updateDto.Email;
            if (updateDto.Phone != null) existingBuyer.Phone = updateDto.Phone;
            if (updateDto.Password != null) existingBuyer.Password = updateDto.Password;


            var update = await _buyerRepo.UpdateAsync(existingBuyer);
            return update != null ? MapToViewDto(update) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _buyerRepo.DeleteAsync(id);
        }

    }
}

