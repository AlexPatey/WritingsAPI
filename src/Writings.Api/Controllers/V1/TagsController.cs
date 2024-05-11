using Asp.Versioning;
using Azure;
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
    public class TagsController(IWritingService writingService, ITagService tagService, ILogger logger) : ControllerBase
    {
        private readonly IWritingService _writingService = writingService;
        private readonly ITagService _tagService = tagService;
        private readonly ILogger _logger = logger;

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

            _logger.LogInformation("Tag with id {Id} created by user id {UserId}", tag.Id, HttpContext.GetUserId());

            var response = tag.MapToReponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet(ApiEndpoints.Tags.Get)]
        [OutputCache]
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

            _logger.LogInformation("Tag with id {Id} deleted by user id {UserId}", id, HttpContext.GetUserId());

            return Ok();
        }

    }
}
