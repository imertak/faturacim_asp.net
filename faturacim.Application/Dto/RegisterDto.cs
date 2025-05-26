
using System.ComponentModel.DataAnnotations;

namespace faturacim.Business.Dto
{
    /// <summary>
    /// Kayıt DTO'su - yeni kullanıcı oluşturmak için kullanılır.
    /// </summary>
    public class RegisterDto
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
