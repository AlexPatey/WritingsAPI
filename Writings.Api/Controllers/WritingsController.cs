using Microsoft.AspNetCore.Mvc;
using Writings.Api.Mappings;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;
using Writings.Application.Services.Interfaces;
using Writings.Contracts.Requests;
using Writings.Contracts.Responses;

namespace Writings.Api.Controllers
{
    [ApiController]
    public class WritingsController(IWritingService writingService) : ControllerBase
    {
        private readonly IWritingService _writingService = writingService;

        [HttpPost(ApiEndpoints.Writings.Create)]
        [ProducesResponseType(typeof(WritingResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateWritingRequest request)
        {
            var writing = request.MapToWriting();

            var created = await _writingService.CreateAsync(writing);

            if (!created)
            {
                return BadRequest();
            }

            var response = writing.MapToResponse();

            return CreatedAtAction(nameof(Get), new { idOrSlug = response.Id }, response);
        }

        [HttpGet(ApiEndpoints.Writings.Get)]
        [ProducesResponseType(typeof(WritingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug)
        {
            var writing = Guid.TryParse(idOrSlug, out var id) ?
                await _writingService.GetByIdAsync(id) :
                await _writingService.GetBySlugAsync(idOrSlug);

            if (writing is null)
            {
                return NotFound();
            }

            var response = writing.MapToResponse();

            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Writings.GetAll)]
        [ProducesResponseType(typeof(WritingsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var writings = await _writingService.GetAllAsync();

            var response = writings.MapToResponse();

            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Writings.GetAllByYear)]
        [ProducesResponseType(typeof(WritingsResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByYear([FromRoute] int year)
        {
            var writings = await _writingService.GetAllByYearAsync(year);

            var response = writings.MapToResponse();

            return Ok(response);
        }

        [HttpPut(ApiEndpoints.Writings.Update)]
        [ProducesResponseType(typeof(WritingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWritingRequest request)
        {
            var writing = request.MapToWriting(id);

            var updated = await _writingService.UpdateAsync(writing);

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
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _writingService.DeleteByIdAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
