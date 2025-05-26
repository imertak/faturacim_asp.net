using faturacim.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace faturacim.Infrastructure.Data
{
    /// <summary>
    /// Entity Framework Core bağlam sınıfı - veritabanı yapılandırması.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Invoice entity’sindeki Amount kolonu için
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(i => i.Amount)
                      .HasPrecision(18, 2); // toplam 18 basamak, 2 ondalık
                                            // Alternatif: .HasColumnType("decimal(18,2)");
            });

            // Diğer entity konfigürasyonlarınız…
        }

    }
}
