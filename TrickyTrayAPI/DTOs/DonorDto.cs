using System.ComponentModel.DataAnnotations;

namespace TrickyTrayAPI.DTOs
{
    public class DonorViewDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Email { get; set; }
        public ICollection<GiftViewDto> Gifts { get; set; } = new List<GiftViewDto>();
    }

    public class DonorCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(256)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
    }

    public class DonorUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        [EmailAddress]
        [MaxLength(256)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }
    }
}
