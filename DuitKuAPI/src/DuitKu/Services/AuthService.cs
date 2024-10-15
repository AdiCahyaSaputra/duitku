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
            var user = new User { Name = dto.Name, Email = dto.Email };
            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return _jwtService.GenerateToken(user.Id, user.Email);
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