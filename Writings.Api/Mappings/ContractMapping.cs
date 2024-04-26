using Writings.Application.Models;
using Writings.Contracts.Requests;
using Writings.Contracts.Responses;

namespace Writings.Api.Mappings
{
    public static class ContractMapping
    {
        #region WritingMappings

        public static Writing MapToWriting(this CreateWritingRequest request)
        {
            return new Writing
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Body = request.Body,
                Type = request.Type,
                YearOfCompletion = request.YearOfCompletion,
                CreatedWhen = DateTimeOffset.Now,
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
                YearOfCompletion = request.YearOfCompletion,
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
                YearOfCompletion = writing.YearOfCompletion,
                CreatedWhen = writing.CreatedWhen,
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

        #endregion

        #region TagMappings

        public static Tag MapToTag(this CreateTagRequest request, Writing writing)
        {
            return new Tag
            {
                Id = Guid.NewGuid(),
                TagName = request.TagName,
                Writing = writing,
                CreatedWhen = DateTimeOffset.Now
            };
        }

        public static TagResponse MapToReponse(this Tag tag)
        {
            return new TagResponse
            {
                Id = tag.Id,
                TagName = tag.TagName,
                Writing = tag.Writing.MapToResponse(),
                CreatedWhen = tag.CreatedWhen
            };
        }

        #endregion
    }
}
