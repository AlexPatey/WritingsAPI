using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Contracts.Enums;

namespace Writings.Contracts.Requests
{
    public class UpdateWritingRequest
    {
        public required string Title { get; init; }
        public required string Body { get; init; }
        public required WritingTypeEnum Type { get; init; }
        public required IList<string> Tags { get; init; } = new List<string>();
        public required int? YearOfCompletion { get; init; }
    }
}
