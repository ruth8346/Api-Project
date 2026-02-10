using System.ComponentModel.DataAnnotations;

namespace TrickyTrayAPI.DTOs
{
    public class GiftViewDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;

        public int Price { get; set; }
        public string DonorName { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    public class GiftCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int Price { get; set; }
        public int? DonorId { get; set; }
        public string? ImageUrl { get; set; }
        public string Category { get; set; } = string.Empty;
    }
    public class GiftUpdateDto
    {
        [Required]
        public int Id { get; set; }


        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int Price { get; set; }
        public int? DonorId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
    }
}
