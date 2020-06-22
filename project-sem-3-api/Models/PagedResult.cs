using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project_sem_3_api.Models
{
    public class PagedResult<T>
    {
        public class PagingInfo
        {
            public int Page { get; set; }

            public int Size { get; set; }

            public int PageCount { get; set; }

            public long Total { get; set; }

        }
        public List<T> Data { get; private set; }

        public PagingInfo Paging { get; private set; }

        public PagedResult(IEnumerable<T> items, int page, int size, long total)
        {
            Data = new List<T>(items);
            Paging = new PagingInfo
            {
                Page = page,
                Size = size,
                Total = total,
                PageCount = total > 0
                    ? (int)Math.Ceiling(total / (double)size)
                    : 0
            };
        }

    }
}