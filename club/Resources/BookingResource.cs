using System;

namespace club.Resources
{
    public class BookingResource
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int CourtId { get; set; }
        public CourtResource Court { get; set; }
        public int MemberId { get; set; }
        public MemberResource Member { get; set; }
    }
}