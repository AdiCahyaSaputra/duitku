using DuitKu.Domain;
using DuitKu.DTOs;
using DuitKu.Persistance.Database;

using Microsoft.AspNetCore.Identity;

namespace DuitKu.Services
{
    public class AuthService(ApplicationDBContext context, JwtService jwtService)
    {
        private readonly ApplicationDBContext _context = context;
        private readonly JwtService _jwtService = jwtService;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public async Task<string> Register(RegisterDto dto)
        {
            var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new User { Name = dto.Name, Email = dto.Email };

                user.Password = _passwordHasher.HashPassword(user, dto.Password);

                await _context.User.AddAsync(user);

                var account = new Account
                {
                    User = user,
                    Name = "Bank Los Santos",
                    Balance = 20918221
                };

                await _context.Account.AddAsync(account);

                List<Category> categories =
                [
                    new Category { User = user, Name = "🍜 Konsumsi" },
                    new Category { User = user, Name = "🚀 Transport" },
                    new Category { User = user, Name = "⚡ Listrik" },
                    new Category { User = user, Name = "🤳 Kuota" },
                ];

                await _context.Category.AddRangeAsync(categories);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return _jwtService.GenerateToken(user.Id, user.Email);
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        public string? Login(LoginDto dto)
        {
            var user = _context.User.FirstOrDefault(user => user.Email == dto.Email);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password) != PasswordVerificationResult.Success)
            {
                return null;
            }

            return _jwtService.GenerateToken(user.Id, user.Email);
        }
    }
}