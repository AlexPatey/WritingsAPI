using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Models;

namespace Writings.Application.Data
{
    public class WritingsContext : DbContext
    {
        public DbSet<Writing> Writings => Set<Writing>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost,1433; Initial Catalog=Writings; User Id=sa; Password=TestPassword#123; TrustServerCertificate=True;");
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
