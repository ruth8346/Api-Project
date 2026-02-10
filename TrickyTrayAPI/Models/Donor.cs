using System.ComponentModel.DataAnnotations;

namespace TrickyTrayAPI.Models
{
    public class Donor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
        public ICollection<Gift> Gifts { get; set; } = new List<Gift>();
    }
}