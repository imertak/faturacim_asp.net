using faturacim.Domain.Entities;

namespace faturacim.Domain.Interfaces
{
    /// <summary>
    /// Fatura veritabanı işlemleri için repository arayüzü.
    /// </summary>
    public interface IInvoiceRepository
    {
        Task<List<Invoice>> GetInvoicesByUserIdAsync(string email);
        Task AddInvoiceAsync(Invoice invoice);
        Task<Invoice?> GetByIdAsync(int id);
    }
}
