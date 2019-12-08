using System.Collections.Generic;

namespace club.Resources
{
    public class MemberListResource : BaseListResource<MemberResource>
    {
        public MemberListResource(IEnumerable<MemberResource> resources, int pageNum, int pageSize, int totalRecords) : base(resources, pageNum, pageSize, totalRecords)
        { }
    }
}
