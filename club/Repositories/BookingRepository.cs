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
        Task<List<Booking>> GetBookings(DateTime startDate, DateTime endDate, int memberId = 0, int courtId = 0);
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
            DateTime startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            _context.Bookings.Where(b => b.DateTime >= startDate && b.DateTime <= endDate);

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
                     
            if (day != default)
            {
                DateTime startDate = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
                DateTime endDate = new DateTime(day.Year, day.Month, day.Day, 23, 59, 59);

                _context.Bookings.Where(b => b.DateTime >= startDate && b.DateTime <= endDate);
            }

            return await _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Court)
                .ThenInclude(c => c.Sport)
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookings(DateTime startDate, DateTime endDate, int memberId = 0, int courtId = 0)
        {
            if (memberId > 0)            
                _context.Bookings.Where(b => b.MemberId == memberId);

            if (courtId > 0)            
                _context.Bookings.Where(b => b.CourtId == courtId);

            if (startDate != default && endDate != default)
            {
                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

                _context.Bookings.Where(b => b.DateTime >= startDate && b.DateTime <= endDate);
            }                

            return await _context.Bookings      
                .AsNoTracking()
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