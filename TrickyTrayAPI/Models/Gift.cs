using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrickyTrayAPI.Models
{
    public class Gift
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public Donor? Donor { get; set; }

        [ForeignKey("Donor")]
        public int? DonorId { get; set; }
        public string? ImageUrl { get; set; }
        public string Category { get; set; } = string.Empty;

        [Required]
        public int Price { get; set; }
        public ICollection<OrderGift> OrderGifts { get; set; } = new List<OrderGift>();

    }
}
