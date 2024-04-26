using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Writings.Application.Models
{
    public class Tag
    {
        public required Guid Id { get; init; }
        public required Writing Writing { get; init; }
        public required string TagName { get; init; }
        public DateTimeOffset CreatedWhen { get; init; }
    }
}
