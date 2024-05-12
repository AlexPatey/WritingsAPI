using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;

namespace Writings.Application.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<bool> CreateAsync(Tag tag, CancellationToken token);
        Task<Tag?> GetAsync(Guid id, CancellationToken token);
        Task<bool> DeleteAsync(Guid id, CancellationToken token);
    }
}
