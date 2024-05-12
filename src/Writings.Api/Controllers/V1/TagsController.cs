using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Writings.Api.Auth;
using Writings.Api.Mappings;
using Writings.Application.Services.Interfaces;
using Writings.Contracts.Requests.V1;
using Writings.Contracts.Responses.V1;
using Writings.Application.Extensions;
using Writings.Application.Models;

namespace Writings.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion(1.0)]
    public class TagsController(IWritingService writingService, ITagService tagService, ILogger<TagsController> logger) : ControllerBase
    {
        private readonly IWritingService _writingService = writingService;
        private readonly ITagService _tagService = tagService;
        private readonly ILogger<TagsController> _logger = logger;

        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        [HttpPost(ApiEndpoints.Tags.Create)]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request, CancellationToken token)
        {
            var writing = await _writingService.GetByIdAsync(request.WritingId, token);

            if (writing is null)
            {
                return BadRequest();
            }

            var tag = request.MapToTag(writing);

            var created = await _tagService.CreateAsync(tag, token);

            if (!created)
            {
                return BadRequest();
            }

            _logger.LogTagCreation(tag.Id, HttpContext.GetUserId());

            var response = tag.MapToReponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet(ApiEndpoints.Tags.Get)]
        [OutputCache]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var tag = await _tagService.GetAsync(id, token);

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
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _tagService.DeleteAsync(id, token);

            if (!deleted)
            {
                return NotFound();
            }

            _logger.LogTagDeletion(id, HttpContext.GetUserId());

            return Ok();
        }

    }
}
