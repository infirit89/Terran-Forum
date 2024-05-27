using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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

            services.AddSingleton<SoftDeleteInterceptor>();

            services.AddDbContext<TerranForumDbContext>((serviceProvider, options) =>
            {
                options
                .UseSqlServer(
                    connectionString, x => x.MigrationsAssembly(assemblyName));
            });

            services.AddScoped<IForumRepository, ForumRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostReplyRepository, PostReplyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRatingRepository<Post>, PostRatingRepository>();

            services.AddScoped<IForumService, ForumService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostReplyService, PostReplyService>();

            services.AddTransient<ISeederService, SeederService>();
            services.AddHostedService<HostedSeederService>();

            return services;
        }
    }
}
