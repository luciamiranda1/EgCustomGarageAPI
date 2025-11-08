using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllAsync(bool includeDeleted = false)
        {
            if (includeDeleted)
                return await _context.Users.ToListAsync();

            return await _context.Users
                                 .Where(u => !u.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id, bool includeDeleted = false)
        {
            if (includeDeleted)
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }
        public async Task<User?> GetByNameAsync(string name, bool includeDeleted = false)
        {
            if (includeDeleted)
                return await _context.Users.FirstOrDefaultAsync(u => u.Name == name);

            return await _context.Users.FirstOrDefaultAsync(u => u.Name == name && !u.IsDeleted);
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user; 
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return false;

            user.IsDeleted = true;
            _context.Users.Update(user);
            var rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
    }
}
