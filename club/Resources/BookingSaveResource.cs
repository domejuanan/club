using System;
using System.ComponentModel.DataAnnotations;

namespace club.Resources
{
    public class BookingSaveResource
    {
        [Required]         
        public DateTime DateTime { get; set; }
        [Required]
        public int CourtId { get; set; }
        [Required]
        public int MemberId { get; set; }
    }
}
