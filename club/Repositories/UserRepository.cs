using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using club.Models;
using System.Linq;
using club.Context;

namespace club.Repositories
{
    public interface IUserRepository
    {
        Task<int> CountAsync();
        Task<IEnumerable<User>> ListAsync(int pageNum = 0, int pageSize = 0);
        Task AddAsync(User user);
        Task<User> FindByIdAsync(int id);
        Task<User> FindByUsername(string username);
        void Update(User user);
        void Remove(User user);
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(ClubDbContext context) : base(context) { }

        public async Task<int> CountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<IEnumerable<User>> ListAsync(int pageNum = 0, int pageSize = 0)
        {
            int skip = (pageNum - 1) * pageSize;

            return await _context.Users
                    .Skip(skip)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> FindByUsername(string username)
        {
            return await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}