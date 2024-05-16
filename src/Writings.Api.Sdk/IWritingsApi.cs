using Refit;
using Writings.Contracts.Requests.V1;
using Writings.Contracts.Responses.V1;

namespace Writings.Api.Sdk
{
    [Headers("Authorization: Bearer")]
    public interface IWritingsApi
    {
        #region WritingsEndpoints

        [Post(ApiEndpoints.Writings.Create)]
        Task<WritingResponse> CreateWritingAsync(CreateWritingRequest request);

        [Get(ApiEndpoints.Writings.Get)]
        Task<WritingResponse> GetWritingAsync(Guid id);

        [Get(ApiEndpoints.Writings.GetAll)]
        Task<WritingsResponse> GetAllWritingsAsync(GetAllWritingsRequest request);

        [Put(ApiEndpoints.Writings.Update)]
        Task<WritingResponse> UpdateWritingAsync(Guid id, UpdateWritingRequest request);

        [Delete(ApiEndpoints.Writings.Delete)]
        Task DeleteWritingAsync(Guid id);

        #endregion

        #region TagEndpoints

        [Post(ApiEndpoints.Tags.Create)]
        Task<TagResponse> CreateTagAsync(CreateTagRequest request);

        [Get(ApiEndpoints.Tags.Get)]
        Task<TagResponse> GetTagAsync(Guid id);

        [Delete(ApiEndpoints.Tags.Delete)]
        Task DeleteTagAsync(Guid id);

        #endregion
    }
}
