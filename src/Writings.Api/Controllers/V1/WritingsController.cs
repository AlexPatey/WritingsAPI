using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Writings.Api.Auth;
using Writings.Api.Mappings;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;
using Writings.Application.Services.Interfaces;
using Writings.Contracts.Requests.V1;
using Writings.Contracts.Responses.V1;

namespace Writings.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion(1.0)]
    public class WritingsController(IWritingService writingService, IOutputCacheStore outputCacheStore) : ControllerBase
    {
        private readonly IWritingService _writingService = writingService;
        private readonly IOutputCacheStore _outputCacheStore = outputCacheStore;

        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPost(ApiEndpoints.Writings.Create)]
        [ProducesResponseType(typeof(WritingResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateWritingRequest request, CancellationToken token)
        {
            var writing = request.MapToWriting();

            var created = await _writingService.CreateAsync(writing, token);

            if (!created)
            {
                return BadRequest();
            }

            await _outputCacheStore.EvictByTagAsync("writings", token);

            var response = writing.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet(ApiEndpoints.Writings.Get)]
        [OutputCache]
        //[ResponseCache(Duration = 30, VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)]
        [ProducesResponseType(typeof(WritingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var writing = await _writingService.GetByIdAsync(id, token);

            if (writing is null)
            {
                return NotFound();
            }

            var response = writing.MapToResponse();

            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Writings.GetAll)]
        [OutputCache(PolicyName = "WritingsCache")]
        //[ResponseCache(Duration = 30, VaryByQueryKeys = ["title", "type", "yearofcompletion", "tagId", "sortBy", "page", "pageSize"], VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)]
        [ProducesResponseType(typeof(WritingsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllWritingsRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var options = request.MapToOptions().WithUserId(userId);

            var writings = await _writingService.GetAllAsync(options, token);

            var writingsCount = await _writingService.GetCountAsync(options.Title, options.Type, options.YearOfCompletion, options.TagId, token);

            var response = writings.MapToResponse(request.Page, request.PageSize, writingsCount);

            return Ok(response);
        }

        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPut(ApiEndpoints.Writings.Update)]
        [ProducesResponseType(typeof(WritingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWritingRequest request, CancellationToken token)
        {
            var writing = request.MapToWriting(id);

            var updated = await _writingService.UpdateAsync(writing, token);

            if (!updated)
            {
                return NotFound();
            }

            await _outputCacheStore.EvictByTagAsync("writings", token);

            var response = writing.MapToResponse();

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpDelete(ApiEndpoints.Writings.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _writingService.DeleteByIdAsync(id, token);

            if (!deleted)
            {
                return NotFound();
            }

            await _outputCacheStore.EvictByTagAsync("writings", token);

            return Ok();
        }
    }
}
