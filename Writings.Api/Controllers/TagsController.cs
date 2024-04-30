using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Writings.Api.Auth;
using Writings.Api.Mappings;
using Writings.Application.Repositories.Interfaces;
using Writings.Application.Services.Interfaces;
using Writings.Contracts.Requests;
using Writings.Contracts.Responses;

namespace Writings.Api.Controllers
{
    [ApiController]
    public class TagsController(IWritingService writingService, ITagService tagService) : ControllerBase
    {
        private readonly IWritingService _writingService = writingService;
        private readonly ITagService _tagService = tagService;

        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPost(ApiEndpoints.Tags.Create)]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            var writing = await _writingService.GetByIdAsync(request.WritingId);

            if (writing is null)
            {
                return BadRequest();
            }

            var tag = request.MapToTag(writing);

            var created = await _tagService.CreateAsync(tag);

            if (!created)
            {
                return BadRequest();
            }

            var response = tag.MapToReponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id}, response);
        }

        [HttpGet(ApiEndpoints.Tags.Get)]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var tag = await _tagService.GetAsync(id);

            if (tag is null)
            {
                return NotFound();
            }

            var response = tag.MapToReponse();

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminUserPolicyName)]
        [HttpDelete(ApiEndpoints.Tags.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _tagService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}
