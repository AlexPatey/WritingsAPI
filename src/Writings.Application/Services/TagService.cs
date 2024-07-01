using FluentValidation;
using Microsoft.Extensions.Logging;
using Writings.Application.Extensions;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;
using Writings.Application.Services.Interfaces;

namespace Writings.Application.Services
{
    public class TagService(ITagRepository tagRepository, IValidator<Tag> tagValidator, ILogger<TagService> logger) : ITagService
    {
        private readonly ITagRepository _tagRepository = tagRepository;
        private readonly IValidator<Tag> _tagValidator = tagValidator;
        private readonly ILogger<TagService> _logger = logger;

        public async Task<bool> CreateAsync(Tag tag, CancellationToken token)
        {
            try
            {
                await _tagValidator.ValidateAndThrowAsync(tag, token);
                return await _tagRepository.CreateAsync(tag, token);
            }
            catch (Exception ex)
            {
                _logger.LogTagCreationFailure(ex.Message);
                throw;
            }
            
        }

        public async Task<Tag?> GetAsync(Guid id, CancellationToken token)
        {
            return await _tagRepository.GetAsync(id, token);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token)
        {
            try
            {
                return await _tagRepository.DeleteAsync(id, token);
            }
            catch (Exception ex)
            {
                _logger.LogTagDeletionFailure(ex.Message);
                throw;
            }
        }
    }
}
