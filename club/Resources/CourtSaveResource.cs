using System.ComponentModel.DataAnnotations;

namespace club.Resources
{
    public class CourtSaveResource
    {        
        [Required]
        [StringLength(20)]
        public string Reference { get; set; }
        [Required]
        public int SportId { get; set; }
    }
}

