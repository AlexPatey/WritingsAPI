using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Contracts.Enums;

namespace Writings.Contracts.Requests
{
    public class GetAllWritingsRequest
    {
        public required string? Title { get; init; }
        public required WritingTypeEnum? Type { get; set; }
        public required int? YearOfCompletion { get; init; }
        public required Guid? TagId { get; init; }
        public required string? SortBy { get; init; }
    }
}
