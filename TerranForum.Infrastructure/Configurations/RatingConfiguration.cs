using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Configurations
{
    internal class PostRatingConfiguration : IEntityTypeConfiguration<Rating<Post>>
    {
        public void Configure(EntityTypeBuilder<Rating<Post>> builder)
        {
            builder.HasKey(e => new { e.UserId, e.ServiceId });
            builder
                .HasOne(e => e.User)
                .WithMany(e => e.PostsRatings)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Service)
                .WithMany(e => e.Ratings)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.IsDeleted);
            builder.HasIndex(x => x.IsDeleted).HasFilter("IsDeleted = 0");
        }
    }

    internal class PostReplyRatingConfiguration : IEntityTypeConfiguration<Rating<PostReply>>
    {
        public void Configure(EntityTypeBuilder<Rating<PostReply>> builder)
        {
            builder.HasKey(e => new { e.UserId, e.ServiceId });
            builder
                .HasOne(e => e.User)
                .WithMany(e => e.PostReplyRatings)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Service)
                .WithMany(e => e.Ratings)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.IsDeleted);
            builder.HasIndex(x => x.IsDeleted).HasFilter("IsDeleted = 0");
        }
    }
}
