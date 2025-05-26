
using System.ComponentModel.DataAnnotations;

namespace faturacim.Domain.Entities
{
    /// <summary>
    /// Kullanıcı varlığı - Fatura sisteminde kullanıcı kimlik bilgilerini temsil eder.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
    }

}
