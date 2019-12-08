using System.ComponentModel.DataAnnotations;

namespace club.Resources
{
    public class SportSaveResource
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}