using System;

namespace club.Resources
{
    public class BookingResource
    {
        public int Id { get; set; }
        public DateTime Reservation { get; set; }        
        public CourtResource Court { get; set; }
        public MemberResource Member { get; set; }
    }
}