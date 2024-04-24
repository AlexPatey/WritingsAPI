using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Writings.Application.Data;
using Writings.Application.Repositories;
using Writings.Application.Repositories.Interfaces;

namespace Writings.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IWritingRepository, WritingRepository>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<WritingsContext>();
            return services;
        }
    }
}
