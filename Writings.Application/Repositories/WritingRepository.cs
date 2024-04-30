using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Data;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;

namespace Writings.Application.Repositories
{
    public class WritingRepository(WritingsContext context) : IWritingRepository
    {
        private readonly WritingsContext _context = context;

        public async Task<bool> CreateAsync(Writing writing, CancellationToken token = default)
        {
            await _context.Writings.AddAsync(writing, token);
            var result = await _context.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<Writing?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var writing = await _context.Writings.FindAsync(id, token);
            return writing;
        }

        public async Task<IEnumerable<Writing>> GetAllAsync(GetAllWritingsOptions options, CancellationToken token = default)
        {
            var filteredWritings = _context.Writings.AsQueryable();

            if (options.Title is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.Title.ToLower().Contains(options.Title));
            }

            if (options.YearOfCompletion is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.YearOfCompletion == options.YearOfCompletion);
            }

            if (options.Type is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.Type == options.Type);
            }

            if (options.TagId is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.Tags.Any(t => t.Id == options.TagId));
            }

            return await filteredWritings.ToListAsync(token);
        }

        public async Task<IEnumerable<Writing>> GetAllByYearAsync(int year, CancellationToken token = default)
        {
            var allWritings = _context.Writings;

            var filteredWritings = allWritings.Where(w => w.YearOfCompletion == year);

            return await filteredWritings.ToListAsync(token);
        }

        public async Task<bool> UpdateAsync(Writing writing, CancellationToken token = default)
        {
            var exists = await _context.Writings.AnyAsync(w => w.Id == writing.Id, token);

            if (!exists)
            {
                return false;
            }

            _context.Writings.Update(writing);

            var result = await _context.SaveChangesAsync(token);

            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            var writing = await _context.Writings.SingleOrDefaultAsync(w => w.Id == id, token);

            if (writing is null)
            {
                return false;
            }

            _context.Writings.Remove(writing);

            var result = await _context.SaveChangesAsync(token);

            return result > 0;
        }
    }
}
