using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Contracts.Enums;

namespace Writings.Contracts.Requests.V1
{
    public class GetAllWritingsRequest : PagedRequest
    {
        public required string? Title { get; init; }
        public required WritingType? Type { get; set; }
        public required int? YearOfCompletion { get; init; }
        public required Guid? TagId { get; init; }
        public required string? SortBy { get; init; }
    }
}
