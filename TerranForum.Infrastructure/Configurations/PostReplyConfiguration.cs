using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Configurations
{
    internal class PostReplyConfiguration : IEntityTypeConfiguration<PostReply>
    {
        public void Configure(EntityTypeBuilder<PostReply> builder)
        {
            builder.HasQueryFilter(x => !x.IsDeleted);
            builder.HasIndex(x => x.IsDeleted).HasFilter("IsDeleted = 0");
        }
    }
}
