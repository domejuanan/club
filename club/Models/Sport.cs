using System.Collections.Generic;

namespace club.Models
{
    public class Sport
    {
        public int Id {get; set;}        
        public string Name { get; set; }
        public ICollection<Court> Courts { get; set; }
    }
}
