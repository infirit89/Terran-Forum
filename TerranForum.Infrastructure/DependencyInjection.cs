using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Application.Repositories;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;
using TerranForum.Infrastructure.Repositories;
using TerranForum.Infrastructure.Services;

namespace TerranForum.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString) 
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            string assemblyName = currentAssembly.GetName().Name ?? throw new NullReferenceException("Infrastructure Assembly name was null");

            services.AddDbContext<TerranForumDbContext>(options =>
            {
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(assemblyName));
            });

            services.AddScoped<IForumRepository, ForumRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostReplyRepository, PostReplyRepository>();

            services.AddTransient<ISeederService, SeederService>();
            services.AddHostedService<HostedSeederService>();

            return services;
        }
    }
}
