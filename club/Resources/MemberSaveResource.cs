using System.ComponentModel.DataAnnotations;

namespace club.Resources
{
    public class MemberSaveResource
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Surname { get; set; }
        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }
        [Required]
        [StringLength(255)]
        public string Address { get; set; }
    }
}