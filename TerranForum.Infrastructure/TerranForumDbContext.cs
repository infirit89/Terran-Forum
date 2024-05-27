using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TerranForum.Domain.Models;
using System.Reflection;
using TerranForum.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TerranForum.Infrastructure
{
    public class TerranForumDbContext : IdentityDbContext<ApplicationUser>
    {
        public TerranForumDbContext(IServiceProvider serviceProvider)
        {
            _Services = serviceProvider;
        }

        public TerranForumDbContext(DbContextOptions<TerranForumDbContext> options, IServiceProvider serviceProvider)
            : base(options)
        {
            _Services = serviceProvider;
        }

        public override DbSet<ApplicationUser> Users { get; set; } = null!;
        public virtual DbSet<Forum> Forums { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Rating<Post>> PostRatings { get; set; } = null!;
        public virtual DbSet<PostReply> PostReplies { get; set; } = null!;
        public virtual DbSet<Rating<PostReply>> PostReplyRatings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_Services.GetRequiredService<SoftDeleteInterceptor>());
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder) 
        {
            Assembly configurationsAssembly = Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(configurationsAssembly);

            base.OnModelCreating(builder);
        }

        public async Task<bool> TrySaveAsync()
        {
            try
            {
                return await SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private readonly IServiceProvider _Services;
    }
}
