using TrickyTrayAPI.DTOs;

namespace TrickyTrayAPI.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginRequestDto login);
    }
}
