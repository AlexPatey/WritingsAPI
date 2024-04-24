using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;

namespace Writings.Application.Repositories
{
    public class WritingRepository : IWritingRepository
    {
        private readonly List<Writing> _writings = [];

        public async Task<bool> CreateAsync(Writing writing)
        {
            _writings.Add(writing);
            return true;
        }

        public async Task<Writing?> GetByIdAsync(Guid id)
        {
            var writing = _writings.SingleOrDefault(w => w.Id == id);
            return writing;
        }

        public async Task<Writing?> GetBySlugAsync(string slug)
        {
            var writing = _writings.SingleOrDefault(w => w.Slug == slug);
            return writing; 
        }

        public async Task<IEnumerable<Writing>> GetAllAsync()
        {
            return _writings.AsEnumerable();
        }

        public async Task<bool> UpdateAsync(Writing writing)
        {
            var writingIndex = _writings.FindIndex(w => w.Id == writing.Id);
            if (writingIndex == -1)
            {
                return false;
            }

            _writings[writingIndex] = writing;
            return true;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var removedCount = _writings.RemoveAll(w => w.Id == id);
            return removedCount > 0;
        }
    }
}
