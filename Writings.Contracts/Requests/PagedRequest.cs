using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Writings.Contracts.Requests
{
    public class PagedRequest
    {
        public required int Page { get; init; } = 1;
        public required int PageSize { get; init; } = 10;
    }
}
