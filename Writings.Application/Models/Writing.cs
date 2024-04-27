using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Writings.Application.Enums;

namespace Writings.Application.Models
{
    public partial class Writing
    {
        public required Guid Id { get; init; }
        public required string Title { get; set; }
        public string Slug => GenerateSlug();
        public required string Body { get; set; }
        public required WritingTypeEnum Type { get; set; }
        public required int? YearOfCompletion { get; set; }

        [JsonIgnore]
        public ICollection<Tag> Tags { get; init; } = new HashSet<Tag>();

        private string GenerateSlug()
        {
            var sluggedTitle = SlugRegex().Replace(Title, string.Empty).ToLower().Replace(" ", "-");
            return $"{sluggedTitle}-{(YearOfCompletion is null ? "unspecified" : YearOfCompletion)}";
        }

        [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
        private static partial Regex SlugRegex();
    }
}
