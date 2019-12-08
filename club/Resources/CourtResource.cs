namespace club.Resources
{
    public class CourtResource
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public int SportId { get; set; }
        public SportResource Sport { get; set; }

    }
}

