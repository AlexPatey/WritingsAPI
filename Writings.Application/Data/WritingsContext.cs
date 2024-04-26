using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Writings.Application.Data.EntityMapping;
using Writings.Application.Models;

namespace Writings.Application.Data
{
    public class WritingsContext(DbContextOptions<WritingsContext> options) : DbContext(options)
    {
        public DbSet<Writing> Writings => Set<Writing>();
        public DbSet<Tag> Tags => Set<Tag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WritingMapping());
            modelBuilder.ApplyConfiguration(new TagMapping());
        }
    }
}
