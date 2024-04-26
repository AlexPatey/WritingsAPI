using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Writings.Application.Data.EntityMapping;
using Writings.Application.Models;

namespace Writings.Application.Data
{
    public class WritingsContext : DbContext
    {
        public DbSet<Writing> Writings => Set<Writing>();
        public DbSet<Tag> Tags => Set<Tag>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost,1433; Initial Catalog=Writings; User Id=sa; Password=TestPassword#123; TrustServerCertificate=True;");
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WritingMapping());
            modelBuilder.ApplyConfiguration(new TagMapping());
        }
    }
}
