using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Writings.Application.Data.EntityMapping;
using Writings.Application.Interceptors;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new WritingsContextSaveChangesInterceptor());
        }
    }
}
