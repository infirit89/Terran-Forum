using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Configurations
{
    internal class ForumConfiguration : IEntityTypeConfiguration<Forum>
    {
        public void Configure(EntityTypeBuilder<Forum> builder)
        {
            builder
                .HasMany(x => x.Posts)
                .WithOne(x => x.Forum)
                .HasForeignKey(x => x.ForumId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
