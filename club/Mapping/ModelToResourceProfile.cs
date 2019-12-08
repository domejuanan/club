using AutoMapper;
using club.Models;
using club.Resources;

namespace club.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Sport, SportResource>();
            CreateMap<Member, MemberResource>();
            CreateMap<Court, CourtResource>();
            CreateMap<User, UserResource>();
            CreateMap<User, UserWithTokenResource>();
            CreateMap<Booking, BookingResource>();
            CreateMap<Court, CourtAvailableResource>();
        }
    }
}