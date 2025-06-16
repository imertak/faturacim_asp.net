using faturacim.Business.Dto;
using faturacim.Business.Interfaces;
using faturacim.Domain.Dtos;
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


        [HttpPost("forgot-password")]
        public async Task ForgotPassword([FromQuery] string email)
        {
            await _authService.ForgetPassword(email);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            return token == null ? Unauthorized("Geçersiz giriş.") : Ok(new { token });
        }

        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo([FromQuery] string email)
        {
            var userInfo = await _authService.GetUserInfoByEmailAsync(email);
            return userInfo != null
                ? Ok(userInfo)
                : NotFound("Kullanıcı bulunamadı.");
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            try
            {
                var result = await _authService.UpdateUserProfile(model);

                if (result)
                    return Ok(new { message = "Profil başarıyla güncellendi" });

                return BadRequest(new { message = "Profil güncellenemedi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
