using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using club.Models;
using System.Linq;
using club.Context;

namespace club.Repositories
{
    public interface IMemberRepository
    {
        Task<int> CountAsync();
        Task<IEnumerable<Member>> ListAsync(int pageNum = 0, int pageSize = 0);
        Task AddAsync(Member member);
        Task<Member> FindByIdAsync(int id);
        void Update(Member member);
        void Remove(Member member);
    }

    public class MemberRepository : BaseRepository, IMemberRepository
    {
        public MemberRepository(ClubDbContext context) : base(context) { }

        public async Task<int> CountAsync()
        {
            return await _context.Members.CountAsync();
        }

        public async Task<IEnumerable<Member>> ListAsync(int pageNum = 0, int pageSize = 0)
        {
            int skip = (pageNum - 1) * pageSize;

            if (pageSize > 0) {
                return await _context.Members
                    .Skip(skip)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }

            return await _context.Members.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Member member)
        {
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        public async Task<Member> FindByIdAsync(int id)
        {
            return await _context.Members.FindAsync(id);
        }

        public void Update(Member member)
        {
            _context.Members.Update(member);
            _context.SaveChanges();
        }

        public void Remove(Member member)
        {
            _context.Members.Remove(member);
            _context.SaveChanges();
        }
    }
}