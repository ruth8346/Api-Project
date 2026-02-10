using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrickyTrayAPI.DTOs;
using TrickyTrayAPI.Models;
using TrickyTrayAPI.Repositories;

namespace TrickyTrayAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IConfiguration _config;

        public AuthService(IBuyerRepository buyerRepository, IConfiguration config)
        {
            _buyerRepository = buyerRepository;
            _config = config;
        }

        public async Task<string?> LoginAsync(LoginRequestDto login)
        {
            var buyer = await _buyerRepository.GetByEmailAsync(login.Email);
            if (buyer == null) return null; // משתמש לא נמצא

            if (buyer.Password != login.Password) return null; // סיסמה שגויה
            return GenerateJwtToken(buyer);
        }

        private string GenerateJwtToken(Buyer buyer)
        {
            var jwtSection = _config.GetSection("Jwt");

            var key = jwtSection.GetValue<string>("Key");
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("JWT Key is missing in configuration.");

            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expires = jwtSection.GetValue<int>("ExpireMinutes");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, buyer.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, buyer.Id.ToString()), // ✅ להוסיף
                new Claim(JwtRegisteredClaimNames.Email, buyer.Email),
                new Claim(ClaimTypes.Role, "manager")
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expires),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
