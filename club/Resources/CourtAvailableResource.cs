using System.Collections.Generic;

namespace club.Resources
{
    public class CourtAvailableResource
    {
        public int Id { get; set; }
        public string Reference { get; set; }       
        public SportResource Sport { get; set; }        
        public List<string> AvailableHours { get; set; }

    }
}
