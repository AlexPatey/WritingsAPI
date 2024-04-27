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
            try
            {
                var writing = await _context.Writings.FindAsync(id);
                return writing;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<Writing?> GetBySlugAsync(string slug)
        {
            var writing = await _context.Writings.SingleOrDefaultAsync(w => w.Slug == slug);
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
            var writingToUpdate = await _context.Writings.SingleOrDefaultAsync(w => w.Id == writing.Id);

            if (writingToUpdate is null)
            {
                return false;
            }

            writingToUpdate.Title = writing.Title;
            writingToUpdate.Body = writing.Body;
            writingToUpdate.Type = writing.Type;
            writingToUpdate.YearOfCompletion = writing.YearOfCompletion;

            _context.Writings.Update(writingToUpdate);

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
