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
    public class TagRepository(WritingsContext writingsContext) : ITagRepository
    {
        private readonly WritingsContext _context = writingsContext;

        public async Task<bool> CreateAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            var tag = await _context.Tags
                .Include(t => t.Writing)
                .SingleOrDefaultAsync(t => t.Id == id);

            return tag;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tag = await _context.Tags.SingleOrDefaultAsync(w => w.Id == id);

            if (tag is null)
            {
                return false;
            }

            _context.Tags.Remove(tag);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

    }
}
