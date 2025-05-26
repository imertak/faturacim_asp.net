using faturacim.Domain.Entities;

namespace faturacim.Domain.Interfaces
{
    /// <summary>
    /// Kullanıcı veritabanı işlemleri için repository arayüzü.
    /// </summary>
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
