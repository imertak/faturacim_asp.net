using System.ComponentModel.DataAnnotations;

namespace faturacim.Domain.Entities
{
    /// <summary>
    /// Fatura varlığı - Kullanıcılara ait fatura kayıtlarını temsil eder.
    /// </summary>
    public class Invoice
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; } = DateTime.UtcNow;

        public string? Category { get; set; }

        public string? ImagePath { get; set; } // Fatura fotoğrafı (dosya yolu)

        public string? PayingStatus { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
