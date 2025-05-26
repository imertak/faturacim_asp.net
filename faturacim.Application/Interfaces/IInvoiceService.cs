using faturacim.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace faturacim.Business.Interfaces
{
    /// <summary>
    /// Fatura işlemlerine dair iş kurallarını tanımlar.
    /// </summary>
    public interface IInvoiceService
    {
        /// <summary>
        /// Belirli bir kullanıcıya ait faturaları tarih aralığına göre getirir.
        /// </summary>
        Task<List<Invoice>> GetUserInvoicesAsync(string email, DateTime? startDate, DateTime? endDate);
        Task<Invoice?> GetInvoiceByIdAsync(int invoiceId);
        Task AddInvoiceAsync(Invoice invoice);
        //Task DeleteInvoiceAsync(int invoiceId);
    }
}
