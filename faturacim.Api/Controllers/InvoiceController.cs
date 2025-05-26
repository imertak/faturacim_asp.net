using faturacim.Business.Interfaces;
using faturacim.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace faturacim.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// Kullanıcının belirli tarih aralığındaki faturalarını getirir.
        /// </summary>
        [HttpGet("user/{email}")]
        public async Task<IActionResult> GetUserInvoices(
            string email,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var invoices = await _invoiceService.GetUserInvoicesAsync(email, startDate, endDate);
            return Ok(invoices);
        }


        /// <summary>
        /// Belirli bir faturayı ID ile getirir.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound("Fatura bulunamadı.");
            return Ok(invoice);
        }

        /// <summary>
        /// Yeni bir fatura ekler.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddInvoice([FromBody] Invoice invoice)
        {
            await _invoiceService.AddInvoiceAsync(invoice);
            return Ok("Fatura başarıyla eklendi.");
        }

        ///// <summary>
        ///// Faturayı siler.
        ///// </summary>
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteInvoice(int id)
        //{
        //    await _invoiceService.DeleteInvoiceAsync(id);
        //    return Ok("Fatura silindi.");
        //}
    }

}
