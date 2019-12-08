using System.Collections.Generic;

namespace club.Models
{
    public class Court
    {
        public int Id { get; set; }
        public string Reference { get; set; }        
        public int SportId { get; set; }
        public Sport Sport { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
