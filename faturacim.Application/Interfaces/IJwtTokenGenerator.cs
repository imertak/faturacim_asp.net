

using faturacim.Domain.Entities;

namespace faturacim.Business.Interfaces
{
    /// <summary>
    /// JWT üretimi için arayüz.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
