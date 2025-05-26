using faturacim.Domain.Entities;
using faturacim.Domain.Interfaces;
using faturacim.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace faturacim.Infrastructure.Repositories
{
    /// <summary>
    /// Fatura verisi için Entity Framework Core repository implementasyonu.
    /// </summary>
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Invoice>> GetInvoicesByUserIdAsync(string email)
        {
            User user = await _context.Users.Where(i => i.Email == email).FirstOrDefaultAsync();

            return await _context.Invoices.Where(i => i.UserId == user.Id).ToListAsync();
        }

        public async Task AddInvoiceAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {
            return await _context.Invoices.FindAsync(id);
        }
    }
}
