using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrickyTrayAPI.Models
{
    public class Buyer
    {
        public enum UserRole
        {
            User = 0,
            Admin = 1
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(9)]
        public string IdentityNumber { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.User;
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

    }
}
