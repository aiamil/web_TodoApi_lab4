using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TodoApi
{
    public class AuthOptions
    {
        public const string ISSUER = "ArchitectureApi";     // издатель токена
        public const string AUDIENCE = "ArchitectureClient"; // потребитель токена
        private const string KEY = "architect_super_secret_key_32_chars_minimum_123!"; // ключ (минимум 32 символа)

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}