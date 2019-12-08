using System.Collections.Generic;

namespace club.Resources
{
    public class BaseListResource<T>
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; } = 0;
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Creates a List Resource.
        /// </summary>
        /// <param name="pageNum">pageNum.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <param name="totalRecords">totalRecords.</param>
        /// <param name="resources">Items.</param>
        /// <returns>Response.</returns>
        protected BaseListResource(IEnumerable<T> resources, int pageNum, int pageSize, int totalItems)
        {
            PageNum = pageNum;
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = resources;
        }
    }
}

