using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Writings.Contracts.Enums;

namespace Writings.Application.Models
{
    public partial class Writing
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public string Slug => GenerateSlug();
        public required string Body { get; init; }
        public required WritingTypeEnum Type { get; init; }
        public required IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
        public required int YearOfUpload { get; init; }
        public required DateTimeOffset UploadedWhen { get; init; }
        public required DateTimeOffset LastEdited { get; init; }

        private string GenerateSlug()
        {
            var sluggedTitle = SlugRegex().Replace(Title, string.Empty).ToLower().Replace(" ", "-");
            return $"{sluggedTitle}-{YearOfUpload}";
        }

        [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
        private static partial Regex SlugRegex();
    }
}
