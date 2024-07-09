using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Writings.Application.Data;
using Writings.Application.Repositories;
using Writings.Application.Repositories.Interfaces;
using Writings.Application.Services;
using Writings.Application.Services.Interfaces;

namespace Writings.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IWritingService, WritingService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IWritingRepository, WritingRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Scoped);
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WritingsContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(connectionString);
            },
            ServiceLifetime.Scoped,
            ServiceLifetime.Singleton);
            return services;
        }
    }
}
