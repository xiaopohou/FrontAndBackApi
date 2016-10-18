using System.Collections.Generic;

namespace Script.I200.Entity.API
{
    public class PaginationBase<T> where T:new()
    {
        public int PageSize { get; set; }

        public int TotalSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPage { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
