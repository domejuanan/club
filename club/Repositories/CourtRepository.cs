using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using club.Models;
using System.Linq;
using club.Context;

namespace club.Repositories
{
    public interface ICourtRepository
    {
        Task<int> CountAsync();
        Task<IEnumerable<Court>> ListAsync(int pageNum = 0, int pageSize = 0);
        Task AddAsync(Court court);
        Task<Court> FindByIdAsync(int id);
        void Update(Court court);
        void Remove(Court court);
    }

    public class CourtRepository : BaseRepository, ICourtRepository
    {
        public CourtRepository(ClubDbContext context) : base(context) { }

        public async Task<int> CountAsync()
        {
            return await _context.Courts.CountAsync();
        }

        public async Task<IEnumerable<Court>> ListAsync(int pageNum = 0, int pageSize = 0)
        {
            if (pageSize > 0)
            {
                int skip = (pageNum - 1) * pageSize;

                return await _context.Courts
                    .Include(c => c.Sport)
                    .Skip(skip)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }

            return await _context.Courts
                .Include(c => c.Sport)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Court court)
        {
            await _context.Courts.AddAsync(court);
            await _context.SaveChangesAsync();
        }

        public async Task<Court> FindByIdAsync(int id)
        {
            return await _context.Courts
                .Where(c => c.Id == id)
                .Include(c => c.Sport)
                .FirstOrDefaultAsync();
        }

        public void Update(Court court)
        {
            _context.Courts.Update(court);
            _context.SaveChanges();

        }

        public void Remove(Court court)
        {
            _context.Courts.Remove(court);
            _context.SaveChanges();
        }
    }
}