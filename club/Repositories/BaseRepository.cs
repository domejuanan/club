using club.Context;

namespace club.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ClubDbContext _context;

        protected BaseRepository(ClubDbContext context)
        {
            _context = context;
        }
    }
}