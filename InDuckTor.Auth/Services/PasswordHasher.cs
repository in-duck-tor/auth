using Microsoft.AspNetCore.Identity;

namespace InDuckTor.Auth.Services
{
    public interface IPasswordHasher
    {
        string GetPasswordHash(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class PasswordHasher : IPasswordHasher
    {
        public string GetPasswordHash(string password)
        {
            return new PasswordHasher<object>().HashPassword(null, password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            var passCheck = new PasswordHasher<object>().VerifyHashedPassword(null, hash, password);

            return ((int)passCheck) == 1;
        }
    }
}
