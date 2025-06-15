using faturacim.Domain.Entities;
using faturacim.Domain.Interfaces;
using faturacim.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace faturacim.Infrastructure.Repositories
{
    /// <summary>
    /// Kullanıcı verisi için Entity Framework Core repository implementasyonu.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserProfileAsync(Domain.Dtos.UpdateProfileDto model)
        {
            try
            {
                var user = await GetByEmailAsync(model.Email);

                if (user == null)
                {
                    return false;
                }

                // Güncelleme işlemleri
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                if (Enum.TryParse<Gender>(model.Gender, out Gender gender))
                {
                    user.Gender = gender;
                }
                user.BirthDate = model.BirthDate;

                _context.Users.Update(user);
                int result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            // Telefon numarası boş veya null ise null döner
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return null;

            // Normalize telefon numarası (boşlukları kaldır)
            var normalizedPhoneNumber = phoneNumber.Replace(" ", "");

            // Veritabanında telefon numarasına göre kullanıcı ara
            return await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.PhoneNumber.Replace(" ", "") == normalizedPhoneNumber
                );
        }
    }

}
