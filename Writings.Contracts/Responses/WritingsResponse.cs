using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Writings.Contracts.Responses
{
    public class WritingsResponse
    {
        public required IEnumerable<WritingResponse> Items { get; init; } = Enumerable.Empty<WritingResponse>();
    }
}
