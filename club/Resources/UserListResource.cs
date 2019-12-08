using System.Collections.Generic;

namespace club.Resources
{
    public class UserListResource : BaseListResource<UserResource>
    {
        public UserListResource(IEnumerable<UserResource> resources, int pageNum, int pageSize, int totalRecords) : base(resources, pageNum, pageSize, totalRecords)
        { }
    }
}
