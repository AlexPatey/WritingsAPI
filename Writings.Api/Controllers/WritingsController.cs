using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Writings.Api.Mappings;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;
using Writings.Application.Services.Interfaces;
using Writings.Contracts.Requests;
using Writings.Contracts.Responses;

namespace Writings.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class WritingsController(IWritingService writingService) : ControllerBase
    {
        private readonly IWritingService _writingService = writingService;

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

            var response = writing.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet(ApiEndpoints.Writings.Get)]
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
        [ProducesResponseType(typeof(WritingsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var writings = await _writingService.GetAllAsync(token);

            var response = writings.MapToResponse();

            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Writings.GetAllByYear)]
        [ProducesResponseType(typeof(WritingsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByYear([FromRoute] int year, CancellationToken token)
        {
            var writings = await _writingService.GetAllByYearAsync(year, token);

            var response = writings.MapToResponse();

            return Ok(response);
        }

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

            var response = writing.MapToResponse();

            return Ok(response);
        }

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

            return Ok();
        }
    }
}
