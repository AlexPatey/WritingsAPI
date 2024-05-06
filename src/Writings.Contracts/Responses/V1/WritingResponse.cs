using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Contracts.Enums;

namespace Writings.Contracts.Responses.V1
{
    public class WritingResponse
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string Body { get; init; }
        public required WritingTypeEnum Type { get; init; }
        public required int? YearOfCompletion { get; init; }
    }
}
