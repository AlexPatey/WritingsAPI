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

        public async Task<bool> CreateAsync(Tag tag, CancellationToken token)
        {
            await _context.Tags.AddAsync(tag, token);
            var result = await _context.SaveChangesAsync(token);
            return result > 0;
        }

        public async Task<Tag?> GetAsync(Guid id, CancellationToken token)
        {
            var tag = await _context.Tags
                .Include(t => t.Writing)
                .SingleOrDefaultAsync(t => t.Id == id, token);

            return tag;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token)
        {
            var tag = await _context.Tags.SingleOrDefaultAsync(w => w.Id == id, token);

            if (tag is null)
            {
                return false;
            }

            _context.Tags.Remove(tag);

            var result = await _context.SaveChangesAsync(token);

            return result > 0;
        }

    }
}
