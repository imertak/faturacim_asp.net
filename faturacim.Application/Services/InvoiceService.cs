using faturacim.Business.Interfaces;
using faturacim.Domain.Entities;
using faturacim.Domain.Interfaces;

namespace faturacim.Business.Services
{
    /// <summary>
    /// Fatura işlemleri iş mantığını içerir.
    /// </summary>
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        /// <summary>
        /// Belirli bir kullanıcıya ait faturaları filtrelenmiş şekilde getirir.
        /// </summary>
        public async Task<List<Invoice>> GetUserInvoicesAsync(string email, DateTime? startDate, DateTime? endDate)
        {
            var invoices = await _invoiceRepository.GetInvoicesByUserIdAsync(email);

            if (startDate.HasValue)
                invoices = invoices.Where(i => i.IssueDate >= startDate.Value).ToList();

            if (endDate.HasValue)
                invoices = invoices.Where(i => i.IssueDate <= endDate.Value).ToList();

            return invoices;
        }


        public async Task<Invoice?> GetInvoiceByIdAsync(int invoiceId)
        {
            return await _invoiceRepository.GetByIdAsync(invoiceId);
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            await _invoiceRepository.AddInvoiceAsync(invoice);
        }

        //public async Task DeleteInvoiceAsync(int invoiceId)
        //{
        //    await _invoiceRepository.DeleteInvoiceAsync(invoiceId);
        //}
    }

}
