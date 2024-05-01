using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Enums;
using Writings.Application.Models;

namespace Writings.Application.Repositories.Interfaces
{
    public interface IWritingRepository
    {
        Task<bool> CreateAsync(Writing writing, CancellationToken token = default);
        Task<Writing?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Writing>> GetAllAsync(GetAllWritingsOptions options, CancellationToken token = default);
        Task<bool> UpdateAsync(Writing writing, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
        Task<int> GetCountAsync(string? title, WritingTypeEnum? type, int? yearOfCompletion, Guid? tagId, CancellationToken token);
    }
}
