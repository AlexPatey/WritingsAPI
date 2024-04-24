using Microsoft.AspNetCore.Mvc;
using Writings.Api.Mappings;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;
using Writings.Contracts.Requests;
using Writings.Contracts.Responses;

namespace Writings.Api.Controllers
{
    [ApiController]
    public class WritingsController : ControllerBase
    {
        private readonly IWritingRepository _writingRepository;

        public WritingsController(IWritingRepository writingRepository)
        {
            _writingRepository = writingRepository;
        }

        [HttpPost(ApiEndpoints.Writings.Create)]
        public async Task<IActionResult> Create([FromBody] CreateWritingRequest request)
        {
            var writing = request.MapToWriting();

            await _writingRepository.CreateAsync(writing);

            var response = writing.MapToResponse();

            return CreatedAtAction(nameof(Get), new { idOrSlug = response.Id }, response);
        }

        [HttpGet(ApiEndpoints.Writings.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug)
        {
            var writing = Guid.TryParse(idOrSlug, out var id) ?
                await _writingRepository.GetByIdAsync(id) :
                await _writingRepository.GetBySlugAsync(idOrSlug);

            if (writing is null)
            {
                return NotFound();
            }

            var response = writing.MapToResponse();

            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Writings.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var writings = await _writingRepository.GetAllAsync();

            var response = writings.MapToResponse();

            return Ok(response);
        }

        [HttpPut(ApiEndpoints.Writings.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWritingRequest request)
        {
            var writing = request.MapToWriting(id);

            var updated = await _writingRepository.UpdateAsync(writing);

            if (!updated)
            {
                return NotFound();
            }

            var response = writing.MapToResponse();

            return Ok(response);
        }

        [HttpDelete(ApiEndpoints.Writings.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _writingRepository.DeleteByIdAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
