using AutoMapper;
using club.Models;
using club.Resources;

namespace club.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SportSaveResource, Sport>();
            CreateMap<MemberSaveResource, Member>();
            CreateMap<CourtSaveResource, Court>();
            CreateMap<UserSaveResource, User>();
            CreateMap<BookingSaveResource, Booking>();
        }
    }
}