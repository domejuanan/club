using System.Collections.Generic;

namespace club.Resources
{
    public class CourtAvailableListResource : BaseListResource<CourtAvailableResource>
    {
        public CourtAvailableListResource(IEnumerable<CourtAvailableResource> resources, int pageNum, int pageSize, int totalRecords) : base(resources, pageNum, pageSize, totalRecords)
        { }
    }
}
