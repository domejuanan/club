using System.Collections.Generic;

namespace club.Resources
{
    public class SportListResource : BaseListResource<SportResource>
    {
        public SportListResource(IEnumerable<SportResource> resources, int pageNum, int pageSize, int totalRecords) : base(resources, pageNum, pageSize, totalRecords)
        { }
    }
}
