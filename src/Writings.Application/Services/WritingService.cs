using FluentValidation;
using Microsoft.Extensions.Logging;
using Writings.Application.Enums;
using Writings.Application.Extensions;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;
using Writings.Application.Services.Interfaces;

namespace Writings.Application.Services
{
    public class WritingService(IWritingRepository writingRepository, IValidator<Writing> writingValidator, IValidator<GetAllWritingsOptions> writingOptionsValidator, 
        ILogger<WritingService> logger) : IWritingService
    {
        private readonly IWritingRepository _writingRepository = writingRepository;
        private readonly IValidator<Writing> _writingValidator = writingValidator;
        private readonly IValidator<GetAllWritingsOptions> _writingOptionsValidator = writingOptionsValidator;
        private readonly ILogger<WritingService> _logger = logger;

        public async Task<bool> CreateAsync(Writing writing, CancellationToken token = default)
        {
            try
            {
                await _writingValidator.ValidateAndThrowAsync(writing, token);
                return await _writingRepository.CreateAsync(writing, token);
            }
            catch (Exception)
            {
                _logger.LogWritingCreationFailure();
                throw;
            }
        }

        public async Task<IEnumerable<Writing>> GetAllAsync(GetAllWritingsOptions options, CancellationToken token = default)
        {
            await _writingOptionsValidator.ValidateAndThrowAsync(options, token);
            return await _writingRepository.GetAllAsync(options, token);
        }

        public async Task<Writing?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return await _writingRepository.GetByIdAsync(id, token);
        }

        public async Task<bool> UpdateAsync(Writing writing, CancellationToken token = default)
        {
            try
            {
                await _writingValidator.ValidateAndThrowAsync(writing, token);
                return await _writingRepository.UpdateAsync(writing, token);
            }
            catch (Exception)
            {
                _logger.LogWritingUpdateFailure();
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                return await _writingRepository.DeleteByIdAsync(id, token);
            }
            catch (Exception)
            {
                _logger.LogWritingDeletionFailure();
                throw;
            }
        }

        public async Task<int> GetCountAsync(string? title, WritingType? type, int? yearOfCompletion, Guid? tagId, CancellationToken token)
        {
            return await _writingRepository.GetCountAsync(title, type, yearOfCompletion, tagId, token);
        }
    }
}
