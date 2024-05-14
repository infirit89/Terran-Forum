using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TerranForum.Domain.Models;
using System.Reflection;

namespace TerranForum.Infrastructure
{
    public class TerranForumDbContext : IdentityDbContext<ApplicationUser>
    {
        public TerranForumDbContext() { }
        public TerranForumDbContext(DbContextOptions<TerranForumDbContext> options)
            : base(options) { }

        public override DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<Forum> Forums { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostReply> PostReplies { get; set; }

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
            catch
            {
                return false;
            }
        }
    }
}
