using System.ComponentModel.DataAnnotations;

namespace club.Resources
{
    public class UserSaveResource
    {
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(80, MinimumLength = 3)]        
        public string Email { get; set; }
        [Required]        
        [StringLength(20, MinimumLength = 12)]
        public string Password { get; set; }
    }
}
