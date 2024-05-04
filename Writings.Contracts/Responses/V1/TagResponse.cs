using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Writings.Contracts.Responses.V1
{
    public class TagResponse
    {
        public required Guid Id { get; init; }
        public required string TagName { get; init; }
        public required WritingResponse Writing { get; init; }
    }
}
