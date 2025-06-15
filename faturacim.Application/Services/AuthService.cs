using faturacim.Business.Dto;
using faturacim.Business.Interfaces;
using faturacim.Domain.Dtos;
using faturacim.Domain.Entities;
using faturacim.Domain.Interfaces;
using System.Reflection;

namespace faturacim.Business.Services
{
    /// <summary>
    /// Kimlik doğrulama servis sınıfı - kayıt ve giriş işlemleri.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthService(IUserRepository userRepository, IJwtTokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            // E-posta kontrolü
            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
                throw new Exception("Bu e-posta adresi zaten kayıtlı.");

            // Telefon numarası kontrolü (opsiyonel)
            if (await _userRepository.GetByPhoneNumberAsync(dto.PhoneNumber) != null)
                throw new Exception("Bu telefon numarası zaten kayıtlı.");

            // Şifre hash'leme
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Cinsiyet enum parse işlemi
            Gender parsedGender;
            if (!Enum.TryParse<Gender>(dto.Gender, true, out parsedGender))
            {
                // Eğer gelen değer "male" veya "female" gibi string ise
                parsedGender = dto.Gender.ToLower() switch
                {
                    "male" => Gender.Male,
                    "female" => Gender.Female,
                    "other" => Gender.Other,
                    _ => Gender.Unspecified
                };
            }



            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                BirthDate = dto.BirthDate,
                PhoneNumber = dto.PhoneNumber,
                Gender = parsedGender,
                CreatedAt = DateTime.UtcNow,
            };

            try
            {
                await _userRepository.AddAsync(user);

                // Opsiyonel: Kayıt sonrası işlemler
                //await _emailService.SendWelcomeEmailAsync(user.Email);


                return true;
            }
            catch (Exception ex)
            {
                // Hata loglaması
                throw new ApplicationException("Kullanıcı kaydı sırasında bir hata oluştu.");
            }
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            user.LastLoginAt = DateTime.UtcNow;
            return _tokenGenerator.GenerateToken(user);
        }

        public async Task<User?> GetUserInfoByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return null;

            return user;

        }


        public async Task<bool> UpdateUserProfile(UpdateProfileDto model)
        {
            // Ek iş kuralları kontrolü
            if (!ValidateUpdateModel(model))
            {
                return false;
            }

            return await _userRepository.UpdateUserProfileAsync(model);
        }

        private bool ValidateUpdateModel(UpdateProfileDto model)
        {
            // Özel iş kuralları kontrolü
            return true;
        }

    }

}
