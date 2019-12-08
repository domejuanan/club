using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using club.Models;
using System.Linq;
using club.Context;

namespace club.Repositories
{
    public interface ISportRepository
    {
        Task<int> CountAsync();
        Task<IEnumerable<Sport>> ListAsync(int pageNum = 0, int pageSize = 0);
        Task AddAsync(Sport sport);
        Task<Sport> FindByIdAsync(int id);
        Task<Sport> FindByName(string name);
        void Update(Sport sport);
        void Remove(Sport sport);
    }

    public class SportRepository : BaseRepository, ISportRepository
    {
        public SportRepository(ClubDbContext context) : base(context) { }

        public async Task<int> CountAsync()
        {
            return await _context.Sports.CountAsync();
        }

        public async Task<IEnumerable<Sport>> ListAsync(int pageNum = 0, int pageSize = 0)
        {
            int skip = (pageNum - 1) * pageSize;

            if (pageSize > 0) {
                return await _context.Sports
                    .Skip(skip)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }

            return await _context.Sports.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Sport sport)
        {
            await _context.Sports.AddAsync(sport);
            await _context.SaveChangesAsync();
        }

        public async Task<Sport> FindByIdAsync(int id)
        {
            return await _context.Sports.FindAsync(id);
        }

        public async Task<Sport> FindByName(string name)
        {
            return await _context.Sports.Where(x => x.Name == name).SingleOrDefaultAsync();
        }

        public void Update(Sport sport)
        {
            _context.Sports.Update(sport);
            _context.SaveChanges();
        }

        public void Remove(Sport sport)
        {
            _context.Sports.Remove(sport);
            _context.SaveChanges();
        }
    }
}