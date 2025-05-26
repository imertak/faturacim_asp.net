using faturacim.Business.Dto;

namespace faturacim.Business.Interfaces
{
    /// <summary>
    /// Kimlik doğrulama servisi arayüzü.
    /// </summary>
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
    }
}
