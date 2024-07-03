using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Data.Common;
using Testcontainers.MsSql;
using Writings.Application.Data;
using Xunit;

namespace Writings.Api.Tests.Integration
{
    public class WritingsApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var connectionString = _msSqlContainer.GetConnectionString();
            connectionString = connectionString.Replace("master", "Writings"); //Modify connection string so that Writings Db is created

            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<WritingsContext>));
                services.RemoveAll(typeof(DbConnection));
                services.AddDbContext<WritingsContext>((_, option) => option.UseSqlServer(connectionString));
            });
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<WritingsContext>();
            dbContext.Database.Migrate();
        }

        public new async Task DisposeAsync()
        {
            await _msSqlContainer.StopAsync();
            await _msSqlContainer.DisposeAsync();
        }
    }
}
