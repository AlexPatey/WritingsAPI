using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;
using Writings.Application.Services.Interfaces;

namespace Writings.Application.Services
{
    public class WritingService(IWritingRepository writingRepository) : IWritingService
    {
        private readonly IWritingRepository _writingRepository = writingRepository;

        public async Task<bool> CreateAsync(Writing writing)
        {
            return await _writingRepository.CreateAsync(writing);
        }

        public async Task<IEnumerable<Writing>> GetAllAsync()
        {
            return await _writingRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Writing>> GetAllByYearAsync(int year)
        {
            return await _writingRepository.GetAllByYearAsync(year);
        }

        public async Task<Writing?> GetByIdAsync(Guid id)
        {
            return await _writingRepository.GetByIdAsync(id);
        }

        public async Task<Writing?> GetBySlugAsync(string slug)
        {
            return await _writingRepository.GetBySlugAsync(slug);
        }

        public async Task<bool> UpdateAsync(Writing writing)
        {
            return await _writingRepository.UpdateAsync(writing);
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            return await _writingRepository.DeleteByIdAsync(id);
        }
    }
}
