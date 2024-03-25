using InDuckTor.Auth.Infrastructure.Database;
using InDuckTor.Auth.Infrastructure.Model;
using InDuckTor.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace InDuckTor.Auth.Services
{
    public interface ICredentialsService
    {
        Task<long> CreateCredentials(CredentialsDto dto);
        Task<long?> CheckCredentials(CredentialsDto dto);
    }

    public class CredentialsService : ICredentialsService
    {
        private readonly AuthDbContext _context;
        private readonly IPasswordHasher _hasher;

        public CredentialsService(AuthDbContext context, IPasswordHasher hasher) { 
            _context = context; 
            _hasher = hasher;
        }

        public async Task<long> CreateCredentials(CredentialsDto dto)
        {
            var exising = await _context.Credentials.FirstOrDefaultAsync(c => c.Login == dto.Login);

            if (exising != null) throw new BadHttpRequestException("Пользователь существует");

            var credentials = new Credentials
            {
                Login = dto.Login,
                PasswordHash = _hasher.GetPasswordHash(dto.Password)
            };

            await _context.AddAsync(credentials);
            await _context.SaveChangesAsync();

            return credentials.Id;
        }

        public async Task<long?> CheckCredentials(CredentialsDto dto)
        {
            var credentials = await _context.Credentials.FirstOrDefaultAsync(c => c.Login == dto.Login);

            if (credentials is null ||  !_hasher.VerifyPassword(dto.Password, credentials.PasswordHash)) return null;

            return credentials.Id;
        }
    }
}
