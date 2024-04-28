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

        public async Task<bool> CreateAsync(Writing writing)
        {
            await _context.Writings.AddAsync(writing);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Writing?> GetByIdAsync(Guid id)
        {
            var writing = await _context.Writings.FindAsync(id);
            return writing;
        }

        public async Task<IEnumerable<Writing>> GetAllAsync()
        {
            return await _context.Writings.ToListAsync();
        }

        public async Task<IEnumerable<Writing>> GetAllByYearAsync(int year)
        {
            var allWritings = _context.Writings;

            var filteredWritings = allWritings.Where(w => w.YearOfCompletion == year);

            return await filteredWritings.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Writing writing)
        {
            var exists = await _context.Writings.AnyAsync(w => w.Id == writing.Id);

            if (!exists)
            {
                return false;
            }

            _context.Writings.Update(writing);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var writing = await _context.Writings.SingleOrDefaultAsync(w => w.Id == id);

            if (writing is null)
            {
                return false;
            }

            _context.Writings.Remove(writing);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
