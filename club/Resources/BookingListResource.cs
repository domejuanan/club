using System.Collections.Generic;

namespace club.Resources
{
    public class BookingListResource : BaseListResource<BookingResource>
    {
        public BookingListResource(IEnumerable<BookingResource> resources, int pageNum, int pageSize, int totalRecords) : base(resources, pageNum, pageSize, totalRecords)
        { }
    }
}
