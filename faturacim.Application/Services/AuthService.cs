using faturacim.Business.Dto;
using faturacim.Business.Interfaces;
using faturacim.Domain.Dtos;
using faturacim.Domain.Entities;
using faturacim.Domain.Interfaces;
using System.Net.Mail;
using System.Net;
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
                    "erkek" => Gender.Male,
                    "kadın" => Gender.Female,
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


                string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "WelcomeEmail.html");

                // Dosyayı oku
                string htmlTemplate = await File.ReadAllTextAsync(htmlPath);

                // [İsim] kısmını dinamik olarak doldur
                string htmlBody = htmlTemplate.Replace("[İsim]", dto.FullName);


                // Opsiyonel: Kayıt sonrası işlemler
                //await _emailService.SendWelcomeEmailAsync(user.Email);
                string smtpHost = "smtp.gmail.com"; // SMTP sunucu adresi
                int smtpPort = 587; // SMTP portu (Gmail için 587)
                string smtpUser = "imertakpinar0@gmail.com";
                string smtpPass = "nika dxgy zlpn djbx"; // Gmail için uygulama şifresi gerekebilir

                // Gönderilecek maili oluştur
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("imertakpinar0@gmail.com", "Mert Akpınar");
                mail.To.Add(dto.Email);
                mail.Subject = "Hoş Geldiniz";
                mail.Body = htmlBody;
                mail.IsBodyHtml = true; // HTML göndermek için true yapabilirsin

                // SMTP client'ı ayarla
                SmtpClient smtp = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true, // TLS kullanılır
                    Credentials = new NetworkCredential(smtpUser, smtpPass)
                };

                try
                {
                    smtp.Send(mail);
                    Console.WriteLine("Mail gönderildi!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata oluştu: " + ex.Message);
                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                // Hata loglaması
                throw new ApplicationException("Kullanıcı kaydı sırasında bir hata oluştu.");
            }
        }

        public async Task ForgetPassword(string email)
        {
            string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "ForgetPassword.html");

            // Dosyayı oku
            string htmlTemplate = await File.ReadAllTextAsync(htmlPath);


            string smtpHost = "smtp.gmail.com"; // SMTP sunucu adresi
            int smtpPort = 587; // SMTP portu (Gmail için 587)
            string smtpUser = "imertakpinar0@gmail.com";
            string smtpPass = "nika dxgy zlpn djbx"; // Gmail için uygulama şifresi gerekebilir

            // Gönderilecek maili oluştur
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("imertakpinar0@gmail.com", "Mert Akpınar");
            mail.To.Add(email);
            mail.Subject = "Hoş Geldiniz";
            mail.Body = htmlTemplate;
            mail.IsBodyHtml = true; // HTML göndermek için true yapabilirsin

            // SMTP client'ı ayarla
            SmtpClient smtp = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true, // TLS kullanılır
                Credentials = new NetworkCredential(smtpUser, smtpPass)
            };

            try
            {
                smtp.Send(mail);
                Console.WriteLine("Mail gönderildi!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata oluştu: " + ex.Message);
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
