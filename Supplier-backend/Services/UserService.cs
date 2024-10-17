using Supplier_backend.Data;
using Supplier_backend.Models;
using Supplier_backend.Utils;
using Supplier_backend.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Supplier_backend.Services
{
    public class UserService
    {
        private readonly SupplierDbContext _context;

        public UserService(SupplierDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID");
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> RegisterUserAsync(string username, string password, string role)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (existingUser != null) throw new InvalidOperationException("Username already exists");

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

        public async Task UpdateUserAsync(int userId, UserUpdateDto userDto)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null) throw new KeyNotFoundException("User not found");

            if (!string.IsNullOrEmpty(userDto.Username)) existingUser.Username = userDto.Username;
            if (!string.IsNullOrEmpty(userDto.Role)) existingUser.Role = userDto.Role;
            if (!string.IsNullOrEmpty(userDto.NewPassword))
            {
                if (string.IsNullOrEmpty(userDto.OldPassword) ||
                    !PasswordHasher.VerifyPassword(userDto.OldPassword, existingUser.PasswordHash))
                {
                    throw new ArgumentException("Old password is incorrect");
                }
                existingUser.PasswordHash = PasswordHasher.HashPassword(userDto.NewPassword);
            }

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
