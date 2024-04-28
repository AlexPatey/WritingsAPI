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

        public async Task<IEnumerable<Writing>> GetAllAsync(CancellationToken token = default)
        {
            return await _context.Writings.ToListAsync(token);
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
