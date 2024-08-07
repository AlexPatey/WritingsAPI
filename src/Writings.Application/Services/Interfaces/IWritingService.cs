﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Enums;
using Writings.Application.Models;

namespace Writings.Application.Services.Interfaces
{
    public interface IWritingService
    {
        Task<bool> CreateAsync(Writing writing, CancellationToken token = default);
        Task<Writing?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<IEnumerable<Writing>> GetAllAsync(GetAllWritingsOptions options, CancellationToken token = default);
        Task<bool> UpdateAsync(Writing writing, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
        Task<int> GetCountAsync(string? title, WritingType? type, int? yearOfCompletion, Guid? tagId, CancellationToken token = default);
    }
}
