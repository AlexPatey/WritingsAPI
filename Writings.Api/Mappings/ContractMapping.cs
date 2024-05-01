using System.Reflection;
using Writings.Application.Enums;
using Writings.Application.Models;
using Writings.Contracts.Enums;
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
                Type = (Application.Enums.WritingTypeEnum)request.Type,
                YearOfCompletion = request.YearOfCompletion
            };
        }

        public static Writing MapToWriting(this UpdateWritingRequest request, Guid id)
        {
            return new Writing
            {
                Id = id,
                Title = request.Title,
                Body = request.Body,
                Type = (Application.Enums.WritingTypeEnum)request.Type,
                YearOfCompletion = request.YearOfCompletion
            };
        }

        public static WritingResponse MapToResponse(this Writing writing)
        {
            return new WritingResponse
            {
                Id = writing.Id,
                Title = writing.Title,
                Body = writing.Body,
                Type = (Contracts.Enums.WritingTypeEnum)writing.Type,
                YearOfCompletion = writing.YearOfCompletion
            };
        }

        public static WritingsResponse MapToResponse(this IEnumerable<Writing> writings, int page, int pageSize, int totalCount)
        {
            return new WritingsResponse
            {
                Items = writings.Select(MapToResponse),
                Page = page,
                PageSize = pageSize,
                Total = totalCount
            };
        }

        public static GetAllWritingsOptions MapToOptions(this GetAllWritingsRequest request)
        {
            return new GetAllWritingsOptions
            {
                Title = request.Title,
                YearOfCompletion = request.YearOfCompletion,
                Type = (Application.Enums.WritingTypeEnum?)request.Type,
                TagId = request.TagId,
                SortField = request.SortBy?.TrimStart('+', '-').Trim(),
                SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                    request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        public static GetAllWritingsOptions WithUserId(this GetAllWritingsOptions options, Guid? userId)
        {
            options.UserId = userId;
            return options;
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
            };
        }

        public static TagResponse MapToReponse(this Tag tag)
        {
            return new TagResponse
            {
                Id = tag.Id,
                TagName = tag.TagName,
                Writing = tag.Writing.MapToResponse()
            };
        }

        #endregion
    }
}
