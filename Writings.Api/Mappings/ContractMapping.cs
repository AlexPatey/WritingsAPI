using Writings.Application.Models;
using Writings.Contracts.Requests;
using Writings.Contracts.Responses;

namespace Writings.Api.Mappings
{
    public static class ContractMapping
    {
        public static Writing MapToWriting(this CreateWritingRequest request)
        {
            return new Writing
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Body = request.Body,
                Type = request.Type,
                Tags = request.Tags,
                YearOfUpload = request.YearOfUpload,
                UploadedWhen = DateTimeOffset.Now,
                LastEdited = DateTimeOffset.Now
            };
        }

        public static Writing MapToWriting(this UpdateWritingRequest request, Guid id)
        {
            return new Writing
            {
                Id = id,
                Title = request.Title,
                Body = request.Body,
                Type = request.Type,
                Tags = request.Tags,
                YearOfUpload = request.YearOfUpload,
                UploadedWhen = DateTimeOffset.Now,
                LastEdited = DateTimeOffset.Now
            };
        }

        public static WritingResponse MapToResponse(this Writing writing)
        {
            return new WritingResponse
            {
                Id = writing.Id,
                Title = writing.Title,
                Slug = writing.Slug,
                Body = writing.Body,
                Type = writing.Type,
                Tags = writing.Tags,
                YearOfUpload = writing.YearOfUpload,
                UploadedWhen = writing.UploadedWhen,
                LastEdited = writing.LastEdited
            };
        }

        public static WritingsResponse MapToResponse(this IEnumerable<Writing> writings)
        {
            return new WritingsResponse
            {
                Items = writings.Select(MapToResponse)
            };
        }
    }
}
