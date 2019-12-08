using System.Collections.Generic;

namespace club.Resources
{
    public class CourtListResource : BaseListResource<CourtResource>
    {
        public CourtListResource(IEnumerable<CourtResource> resources, int pageNum, int pageSize, int totalRecords) : base(resources, pageNum, pageSize, totalRecords)
        { }
    }
}
