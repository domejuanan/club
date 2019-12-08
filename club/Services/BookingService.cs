using System;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using club.Models;
using club.Repositories;
using club.Resources;
using club.Responses;

namespace club.Services
{
    public interface IBookingService
    {
        Task<BookingListResponse> ListAsync(int pageNum = 1, int pageSize = 50, string dateTimeString = null);
        Task<BookingResponse> GetAsync(int id);
        Task<BookingResponse> SaveAsync(BookingSaveResource bookingSaveResource);
        Task<BookingResponse> UpdateAsync(int id, BookingSaveResource bookingSaveResource);
        Task<BookingResponse> DeleteAsync(int id);
    }

    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICourtRepository _courtRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public BookingService(IBookingRepository bookingRepository, ICourtRepository courtRepository, IMemberRepository memberRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _courtRepository = courtRepository;
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<BookingListResponse> ListAsync(int pageNum = 1, int pageSize = 50, string dateTimeString = null)
        {
            DateTime dateTime = default;
            
            if (!String.IsNullOrWhiteSpace(dateTimeString) && !DateTime.TryParse(dateTimeString, out dateTime))
            {
                return new BookingListResponse(400, "Wrong date format", "dateTimeString", "dateTimeString format has to be: yyyy-MM-ddTHH:mm:ss");
            }
            
            if (pageNum < 1 || pageSize < 1) 
                return new BookingListResponse(400, "Wrong pagination", "Pagination", "pageNum and pageSize params must be greater than zero.");

            int totalRecords;

            if (dateTime != default)
            {
                totalRecords = await _bookingRepository.CountDayBookingsAsync(dateTime);
            }
            else
            {
                totalRecords = await _bookingRepository.CountAsync();
            }
            
            var bookings = await _bookingRepository.ListBookings(dateTime, pageNum, pageSize);            
            var resources = _mapper.Map<IEnumerable<Booking>, IEnumerable<BookingResource>>(bookings);
            var resourceList = new BookingListResource(resources, pageNum, pageSize, totalRecords);
            
            return new BookingListResponse(resourceList);
        }

        public async Task<BookingResponse> GetAsync(int id)
        {
            var existingItem = await _bookingRepository.FindByIdAsync(id);

            if (existingItem == null)
                return new BookingResponse(404,"Item id not found", "Id", "Booking id not found.");

            var responseResource = _mapper.Map<Booking, BookingResource>(existingItem);
            return new BookingResponse(responseResource);
        }

        public async Task<BookingResponse> SaveAsync(BookingSaveResource bookingSaveResource)
        {
            try
            {
                BookingResponse bookingResponse = await CommonValidations(bookingSaveResource);

                if (bookingResponse != null)
                    return bookingResponse;

                var item = _mapper.Map<BookingSaveResource, Booking>(bookingSaveResource);
                await _bookingRepository.AddAsync(item);
                var responseResource = _mapper.Map<Booking, BookingResource>(item);
                return new BookingResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new BookingResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        public async Task<BookingResponse> UpdateAsync(int id, BookingSaveResource bookingSaveResource)
        {
            var booking = await _bookingRepository.FindByIdAsync(id);

            if (booking == null)
                return new BookingResponse(404, "Item id not found", "Id","Booking id not found.");

            BookingResponse bookingResponse = await CommonValidations(bookingSaveResource);

            if (bookingResponse != null)
                return bookingResponse;

            booking.MemberId = bookingSaveResource.MemberId;
            booking.CourtId = bookingSaveResource.CourtId;
            booking.Reservation = bookingSaveResource.Reservation;

            try
            {
                _bookingRepository.Update(booking);
                var responseResource = _mapper.Map<Booking, BookingResource>(booking);
                return new BookingResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new BookingResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        public async Task<BookingResponse> DeleteAsync(int id)
        {
            var existingItem = await _bookingRepository.FindByIdAsync(id);
            

            if (existingItem == null)
                return new BookingResponse(404, "Item id not found", "Id","Booking id not found.");

            try
            {
                _bookingRepository.Remove(existingItem);
                var responseResource = _mapper.Map<Booking, BookingResource>(existingItem);                
                return new BookingResponse(responseResource);
            }
            catch (Exception ex)
            {
                return new BookingResponse(400, "Unexpected error", "Error", ex.Message);
            }
        }

        private async Task<BookingResponse> CommonValidations(BookingSaveResource bookingSaveResource)
        {
            var court = await _courtRepository.FindByIdAsync(bookingSaveResource.CourtId);

            if (court == null)
                return new BookingResponse(404, "Item id not found", "CourtId", "Court id not found.");

            var member = await _memberRepository.FindByIdAsync(bookingSaveResource.MemberId);

            if (member == null)
                return new BookingResponse(404, "Item id not found", "MemberId", "Member id not found.");

            if (bookingSaveResource.Reservation.Hour < 8 || bookingSaveResource.Reservation.Hour > 22)
                return new BookingResponse(400, "Bad booking time", "DateTime", "Bookings hour must be between 08:00 and 22:00");

            if (bookingSaveResource.Reservation.Minute != 0 || bookingSaveResource.Reservation.Second != 0 || bookingSaveResource.Reservation.Millisecond != 0)
                return new BookingResponse(400, "Bad booking time", "DateTime", "Bookings only accept full hours");

            if (bookingSaveResource.Reservation < DateTime.Today)
                return new BookingResponse(400, "Bad booking time", "DateTime", "DateTime is older than today");                       

            var bookingsOfCourt = await _bookingRepository.GetBookingsOfHour(bookingSaveResource.Reservation, 0, bookingSaveResource.CourtId);

            if (bookingsOfCourt.Count > 0)
                return new BookingResponse(400, "Court already booked", "CourtId", "Court is already booked for " + bookingSaveResource.Reservation.ToString("yyyy-MM-dd HH:mm"));
            
            var bookingsOfMember = await _bookingRepository.GetBookingsOfDate(bookingSaveResource.Reservation, bookingSaveResource.MemberId);

            if (bookingsOfMember.Count == 3)
                return new BookingResponse(400, "Too many bookings", "MemberId", "A member only is allowed to book 3 courts per day");

            var bookingsOfMemberHour = await _bookingRepository.GetBookingsOfHour(bookingSaveResource.Reservation, bookingSaveResource.MemberId);

            if (bookingsOfMemberHour.Count == 2)
                return new BookingResponse(400, "Too many bookings today", "MemberId", "A member only is allowed to book 2 courts at the same hour");

            return null;
        }
    }
}