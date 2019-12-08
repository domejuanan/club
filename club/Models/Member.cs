using System.Collections.Generic;

namespace club.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
