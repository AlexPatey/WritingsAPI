using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Data;
using Writings.Application.Enums;
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
                filteredWritings = filteredWritings.Where(w => w.Title.Contains(options.Title, StringComparison.OrdinalIgnoreCase));
            }

            if (options.Type is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.Type == options.Type);
            }

            if (options.YearOfCompletion is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.YearOfCompletion == options.YearOfCompletion);
            }

            if (options.TagId is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.Tags.Any(t => t.Id == options.TagId));
            }

            if (options.SortField is not null)
            {
                switch (options.SortField)
                {
                    case "title":
                        filteredWritings = options.SortOrder is Enums.SortOrder.Ascending ? filteredWritings.OrderBy(w => w.Title) : 
                            filteredWritings.OrderByDescending(w => w.Title);
                        break;
                    case "type":
                        filteredWritings = options.SortOrder is Enums.SortOrder.Ascending ? filteredWritings.OrderBy(w => w.Type) : 
                            filteredWritings.OrderByDescending(w => w.Type);
                        break;
                    case "yearofcompletion":
                        filteredWritings = options.SortOrder is Enums.SortOrder.Ascending ? filteredWritings.OrderBy(w => w.YearOfCompletion) : 
                            filteredWritings.OrderByDescending(w => w.YearOfCompletion);
                        break;
                }
            }
            else
            {
                filteredWritings = filteredWritings.OrderByDescending(w => EF.Property<DateTimeOffset>(w, "CreatedWhen"));
            }

            filteredWritings = filteredWritings.Skip((options.Page - 1) * options.PageSize).Take(options.PageSize);

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

        public async Task<int> GetCountAsync(string? title, WritingType? type, int? yearOfCompletion, Guid? tagId, CancellationToken token)
        {
            var filteredWritings = _context.Writings.AsQueryable();

            if (title is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }

            if (type is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.Type == type);
            }

            if (yearOfCompletion is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.YearOfCompletion == yearOfCompletion);
            }

            if (tagId is not null)
            {
                filteredWritings = filteredWritings.Where(w => w.Tags.Any(t => t.Id == tagId));
            }

            return await filteredWritings.CountAsync(token);
        }
    }
}
