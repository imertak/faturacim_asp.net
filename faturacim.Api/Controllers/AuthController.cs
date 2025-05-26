using faturacim.Business.Dto;
using faturacim.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace faturacim.Api.Controllers
{
    /// <summary>
    /// Kullanıcı kimlik doğrulama işlemlerini sağlayan Web API Controller'ı.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var success = await _authService.RegisterAsync(dto);
            return success ? Ok("Kayıt başarılı") : BadRequest("Kullanıcı zaten mevcut.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            return token == null ? Unauthorized("Geçersiz giriş.") : Ok(new { token });
        }
    }
}
