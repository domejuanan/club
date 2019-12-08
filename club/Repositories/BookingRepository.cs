using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using club.Models;
using System.Linq;
using club.Context;

namespace club.Repositories
{
    public interface IBookingRepository
    {
        Task<int> CountDayBookingsAsync(DateTime date);
        Task<int> CountAsync();
        Task AddAsync(Booking booking);
        Task<Booking> FindByIdAsync(int id);
        Task<List<Booking>> ListBookings(DateTime day, int pageNum = 1, int pageSize = 50);
        Task<List<Booking>> GetBookingsOfHour(DateTime dateTime, int memberId = 0, int courtId = 0);
        Task<List<Booking>> GetBookingsOfDate(DateTime date, int memberId = 0, int courtId = 0);
        void Update(Booking booking);
        void Remove(Booking booking);
        List<String> GetHourSlots();
    }

    public class BookingRepository : BaseRepository, IBookingRepository
    {
        public BookingRepository(ClubDbContext context) : base(context) { }

        public async Task<int> CountAsync()
        {
            return await _context.Bookings.CountAsync();
        }

        public async Task<int> CountDayBookingsAsync(DateTime date)
        {
            _context.Bookings.Where(b => b.Reservation == date.Date);
            return await _context.Bookings.CountAsync();
        }
        
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> ListBookings(DateTime day, int pageNum = 1, int pageSize = 50)
        {
            int skip = (pageNum - 1) * pageSize;            
            IQueryable<Booking> q = _context.Bookings;
            
            if (day != default)
                q = q.Where(b => b.Reservation == day.Date);

            return await q.Include(b => b.Member)
                .Include(b => b.Court)
                .ThenInclude(c => c.Sport)
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<List<Booking>> GetBookingsOfHour(DateTime dateTime, int memberId = 0, int courtId = 0)
        {
            IQueryable<Booking> q = _context.Bookings;

            if (memberId > 0)
                q = q.Where(b => b.MemberId == memberId);

            if (courtId > 0)
                q = q.Where(b => b.CourtId == courtId);

            if (dateTime != default)
                q = q.Where(b => b.Reservation.Date == dateTime.Date && b.Reservation.Hour == dateTime.Hour);
            
            return await q.AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsOfDate(DateTime date, int memberId = 0, int courtId = 0)
        {
            IQueryable<Booking> q = _context.Bookings;

            if (memberId > 0)
                q = q.Where(b => b.MemberId == memberId);

            if (courtId > 0)
                q = q.Where(b => b.CourtId == courtId);

            if (date != default)
                _context.Bookings.Where(b => b.Reservation == date.Date);

            return await q.AsNoTracking()
                .ToListAsync();
        }


        public async Task<Booking> FindByIdAsync(int id)
        {
            return await _context.Bookings
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
            _context.SaveChanges();

        }

        public void Remove(Booking booking)
        {
            _context.Bookings.Remove(booking);
            _context.SaveChanges();
        }

        public List<String> GetHourSlots()
        {
            List<String> slots = new List<String>
            {
                "08:00",
                "09:00",
                "10:00",
                "11:00",
                "12:00",
                "13:00",
                "14:00",
                "15:00",
                "16:00",
                "17:00",
                "18:00",
                "19:00",
                "20:00",
                "21:00",
                "22:00"
            };
            return slots;
        }
    }
}