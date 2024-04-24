using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;

namespace Writings.Application.Repositories.Interfaces
{
    public interface IWritingRepository
    {
        Task<bool> CreateAsync(Writing writing);
        Task<Writing> GetByIdAsync(Guid id);
        Task<Writing> GetBySlugAsync(string slug);
        Task<IEnumerable<Writing>> GetAllAsync();
        Task<bool> UpdateAsync(Writing writing);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}
