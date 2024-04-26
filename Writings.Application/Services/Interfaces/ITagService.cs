using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;

namespace Writings.Application.Services.Interfaces
{
    public interface ITagService
    {
        Task<bool> CreateAsync(Tag tag);
        Task<Tag?> GetAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
}
