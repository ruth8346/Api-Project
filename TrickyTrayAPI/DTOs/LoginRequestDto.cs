using System.ComponentModel.DataAnnotations;

namespace TrickyTrayAPI.DTOs
{
    public class LoginRequestDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}