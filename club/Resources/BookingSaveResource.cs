using System;
using System.ComponentModel.DataAnnotations;

namespace club.Resources
{
    public class BookingSaveResource
    {
        [Required]         
        public DateTime Reservation { get; set; }
        [Required]
        public int CourtId { get; set; }
        [Required]
        public int MemberId { get; set; }
    }
}
