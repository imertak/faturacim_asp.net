using faturacim.Business.Dto;
using faturacim.Domain.Dtos;
using faturacim.Domain.Entities;

namespace faturacim.Business.Interfaces
{
    /// <summary>
    /// Kimlik doğrulama servisi arayüzü.
    /// </summary>
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
        Task<User?> GetUserInfoByEmailAsync(string email);
        Task<bool> UpdateUserProfile(UpdateProfileDto model);
        Task ForgetPassword(string email);
    }
}
