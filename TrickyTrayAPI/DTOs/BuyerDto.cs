using System.ComponentModel.DataAnnotations;
using static TrickyTrayAPI.Models.Buyer;

namespace TrickyTrayAPI.DTOs
{
    public class BuyerDto
    {
        public class BuyerViewDto
        {
            public int Id { get; set; }

            [Required]
            [StringLength(9)]
            public string IdentityNumber { get; set; } = default!;

            [Required]
            public UserRole Role { get; set; } = UserRole.User;
            public string Name { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [MaxLength(256)]
            public string Email { get; set; } = default!;

            [Required]
            [Phone]
            public string Phone { get; set; } = default!;
        }

        public class BuyerCreateDto
        {
            [Required]
            [StringLength(9)]
            public string IdentityNumber { get; set; } = default!;

            [Required]
            public UserRole Role { get; set; } = UserRole.User;
            public string Name { get; set; } = string.Empty;

            [Required]
            public string Password { get; set; } = default!;

            [Required]
            [EmailAddress]
            [MaxLength(256)]
            public string Email { get; set; } = default!;

            [Required]
            [Phone]
            public string Phone { get; set; } = default!;
        }

        public class BuyerUpdateDto
        {
            [Required]
            public int Id { get; set; }

            public string? Name { get; set; }
            public string? Password { get; set; }

            [EmailAddress]
            [MaxLength(256)]
            public string? Email { get; set; }

            [Phone]
            public string? Phone { get; set; }
        }
    }
}
