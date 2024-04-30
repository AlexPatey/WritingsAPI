using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Enums;

namespace Writings.Application.Models
{
    public class GetAllWritingsOptions
    {
        public required string? Title { get; init; }
        public required WritingTypeEnum? Type { get; set; }
        public required int? YearOfCompletion { get; init; }
        public required Guid? TagId { get; init; }
        public Guid? UserId { get; set; }
    }
}
