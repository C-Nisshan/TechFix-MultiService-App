using Microsoft.Extensions.Configuration;
using Supplier_backend.Models;
using Supplier_backend.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Supplier_backend.Data;

namespace Supplier_backend.Services
{
    public class AuthService
    {
        private readonly SupplierDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(SupplierDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<User> RegisterUserAsync(string username, string password, string role)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists");
            }

            var passwordHash = PasswordHasher.HashPassword(password);
            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Generate the JWT token
            return JwtTokenHelper.GenerateToken(user, _config);
        }
    }
}
