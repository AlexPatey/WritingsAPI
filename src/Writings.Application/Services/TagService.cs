using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;
using Writings.Application.Repositories.Interfaces;
using Writings.Application.Services.Interfaces;

namespace Writings.Application.Services
{
    public class TagService(ITagRepository tagRepository) : ITagService
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<bool> CreateAsync(Tag tag)
        {
            return await _tagRepository.CreateAsync(tag);
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _tagRepository.GetAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _tagRepository.DeleteAsync(id);
        }
    }
}
