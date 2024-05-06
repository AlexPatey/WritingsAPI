using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Writings.Application.Enums;

namespace Writings.Application.Models
{
    public class Writing
    {
        public required Guid Id { get; init; }
        public required string Title { get; set; }
        public required string Body { get; set; }
        public required WritingTypeEnum Type { get; set; }
        public required int? YearOfCompletion { get; set; }

        [JsonIgnore]
        public byte[] ConcurrencyToken { get; set; } = new byte[0];

        [JsonIgnore]
        public ICollection<Tag> Tags { get; init; } = new HashSet<Tag>();
    }
}
