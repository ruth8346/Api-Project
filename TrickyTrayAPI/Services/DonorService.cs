using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;

namespace TrickyTrayAPI.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _donorRepo;
        private readonly SystemStateService _systemStateService;
        public DonorService(IDonorRepository donorRepo, SystemStateService systemStateService)
        {
            _donorRepo = donorRepo;
            _systemStateService = systemStateService;
        }

        public async Task<IEnumerable<DonorViewDto>> GetAllDonorsAsync()
        {
            // 1. קבלת הנתונים מה-Repository
            var donors = await _donorRepo.GetAllDonorsWithGiftsAsync();

            // 2. המרה של כל תורם ברשימה ל-DTO (Mapping)
            return donors.Select(donor => new DonorViewDto
            {
                Id = donor.Id,
                Name = donor.Name,

                // הוספת השורות האלו היא קריטית:
                Email = donor.Email, // עכשיו האימייל יעבור מה-Database ל-DTO
                Phone = donor.Phone, // עכשיו הטלפון יעבור (בהנחה שהוספת אותו ל-DonorViewDto)

                // המרה של רשימת המתנות
                Gifts = donor.Gifts.Select(g => new GiftViewDto
                {
                    Name = g.Name
                }).ToList()
            }).ToList();
        }

        public async Task<DonorViewDto?> GetByIdAsync(int id)
        {
            var donor = await _donorRepo.GetByIdAsync(id);

            return donor != null ? MapToViewDto(donor) : null;
        }
        private DonorViewDto MapToViewDto(Donor d)
        {
            return new DonorViewDto
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.Email, // חסר היה כאן!
                Phone = d.Phone  // חסר היה כאן!
            };
        }

        public async Task<DonorViewDto> CreateAsync(DonorCreateDto createDto)
        {

            var systemState = _systemStateService.GetState();
            if (systemState.Status != SystemState.SaleStatus.Draft)
            {
                throw new InvalidOperationException("Cannot create donor after the sale has started.");
            }
            var donor = new Donor
            {
                Name = createDto.Name,
                Email = createDto.Email ?? string.Empty,
                Phone = createDto.Phone,
            };


            var savedDonor = await _donorRepo.CreateAsync(donor);


            return MapToViewDto(savedDonor);
        }

        public async Task<DonorViewDto?> UpdateAsync(int id, DonorUpdateDto updateDto)
        {
            var existingDonor = await _donorRepo.GetByIdAsync(id);
            if (existingDonor == null) return null;

            if (updateDto.Name != null) existingDonor.Name = updateDto.Name;
            if (updateDto.Email != null) existingDonor.Email = updateDto.Email;
            if (updateDto.Phone != null) existingDonor.Phone = updateDto.Phone;

            var update = await _donorRepo.UpdateAsync(existingDonor);
            return update != null ? MapToViewDto(update) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _donorRepo.DeleteAsync(id);
        }

        public async Task<List<DonorViewDto>> SearchDonorsAsync(string? name, string? email, string? giftName)
        {
            var donors = await _donorRepo.SearchDonorsAsync(name, email, giftName);

            return donors.Select(d => new DonorViewDto
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.Email,
                Gifts = d.Gifts?.Select(g => new GiftViewDto
                {
                    Id = g.Id,
                    Name = g.Name
                }).ToList() ?? new List<GiftViewDto>()
            }).ToList();
        }



    }
}
