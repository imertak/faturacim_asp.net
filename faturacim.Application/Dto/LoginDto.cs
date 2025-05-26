
using System.ComponentModel.DataAnnotations;

namespace faturacim.Business.Dto
{
    /// <summary>
    /// Giriş DTO'su - kullanıcı oturum açmak için kullanır.
    /// </summary>
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
