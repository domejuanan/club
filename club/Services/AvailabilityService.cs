using System;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using club.Models;
using club.Repositories;
using club.Resources;
using club.Responses;
using System.Linq;


namespace club.Services
{
    public interface IAvailabilityService
    {        
        Task<CourtAvailableListResponse> Availability(string dateTimeString, int sportId, int memberId, int pageNum = 1, int pageSize = 50);
    }

    public class AvailabilityService : IAvailabilityService
    {
        private readonly ISportRepository _sportRepository;
        private readonly ICourtRepository _courtRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public AvailabilityService(ICourtRepository courtRepository, ISportRepository sportRepository, 
            IBookingRepository bookingRepository, IMemberRepository memberRepository, IMapper mapper)
        {
            _courtRepository = courtRepository;
            _sportRepository = sportRepository;
            _bookingRepository = bookingRepository;
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<CourtAvailableListResponse> Availability(string dateTimeString, int sportId, int memberId, int pageNum = 1, int pageSize = 50)
        {
            var sport = await _sportRepository.FindByIdAsync(sportId);

            if (sport == null)
                return new CourtAvailableListResponse(404, "Item id not found", "SportId", "Sport id not found.");

            var member = await _memberRepository.FindByIdAsync(memberId);

            if (member == null)
                return new CourtAvailableListResponse(404, "Item id not found", "MemberId", "Member id not found.");

            DateTime dateTime = default;

            if (!String.IsNullOrWhiteSpace(dateTimeString) && !DateTime.TryParse(dateTimeString, out dateTime))
            {
                return new CourtAvailableListResponse(400, "Wrong date format", "dateTimeString", "dateTimeString format has to be: yyyy-MM-ddTHH:mm:ss");
            }

            if (dateTime < DateTime.Today)
                return new CourtAvailableListResponse(400, "Bad booking time", "DateTime", "DateTime is older than today");

            DateTime dayStartDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 8, 0, 0);
            DateTime dayEndDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 22, 0, 0);

            var bookingsOfMember = await _bookingRepository.GetBookings(dayStartDate, dayEndDate, memberId);

            if (bookingsOfMember.Count == 3)
                return new CourtAvailableListResponse(400, "Too many bookings", "MemberId", "A member only is allowed to book 3 courts per day");

            var bookingsOfMemberHour = await _bookingRepository.GetBookings(dateTime, dateTime, memberId);

            if (bookingsOfMemberHour.Count == 2)
                return new CourtAvailableListResponse(400, "Too many bookings", "MemberId", "A member only is allowed to book 2 courts at the same hour");

            var todayBookings = await _bookingRepository.GetBookings(dayStartDate, dayEndDate);

            var hoursByCourt = todayBookings.GroupBy(b => b.CourtId)
                .ToDictionary(k => k.Key, v => v.Select(f => f.DateTime.ToString("HH:mm")).ToList());

            
            var allCourts = await _courtRepository.ListAsync(pageNum, pageSize);
            var resources = _mapper.Map<IEnumerable<Court>, IEnumerable<CourtAvailableResource>>(allCourts);

            List<String> hours = _bookingRepository.GetHourSlots();

            foreach (CourtAvailableResource court in resources)
            {
                if (hoursByCourt.ContainsKey(court.Id))
                {
                    court.AvailableHours = hours.Except(hoursByCourt[court.Id]).ToList();
                }
                else 
                {
                    court.AvailableHours = hours;
                }
            }
            
            int totalRecords = await _courtRepository.CountAsync();
            var resourceList = new CourtAvailableListResource(resources, pageNum, pageSize, totalRecords);

            return new CourtAvailableListResponse(resourceList);
        }
        
        
    }
}