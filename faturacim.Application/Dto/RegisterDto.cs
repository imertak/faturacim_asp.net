
using System.ComponentModel.DataAnnotations;

namespace faturacim.Business.Dto
{
    /// <summary>
    /// Kayıt DTO'su - yeni kullanıcı oluşturmak için kullanılır.
    /// <summary>
    /// Kullanıcı kayıt bilgileri için Data Transfer Object
    /// </summary>
    public class RegisterDto
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required, Phone]
        [RegularExpression(@"^(05\d{9})$", ErrorMessage = "Geçerli bir telefon numarası girin")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [RegularExpression("^(erkek|kadın|diğer)$", ErrorMessage = "Geçerli bir cinsiyet seçin")]
        public string Gender { get; set; } = string.Empty;
    }
}