using System.ComponentModel.DataAnnotations;

namespace club.Resources
{
    public class UserAuthenticationResource
    {
        [Required]
        [EmailAddress]
        [StringLength(80, MinimumLength = 3)]
        public string Email { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 12)]
        public string Password { get; set; }
    }

}
