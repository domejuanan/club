using System;

namespace club.Models
{
    public class Booking
    {
        public int Id { get; set; }        
        public DateTime Reservation { get; set; }        
        public int CourtId { get; set; }
        public Court Court { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
