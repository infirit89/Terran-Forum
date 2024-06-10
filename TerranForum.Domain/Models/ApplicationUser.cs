using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TerranForum.Domain.Interfaces;

namespace TerranForum.Domain.Models
{
    public class ApplicationUser : IdentityUser, ISoftDeletableEntity
    {
        // ---- soft delete stuffs ----
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        // ----------------------------

        public string ProfileImageUrl { get; set; } = null!;
        public DateTimeOffset JoinedAt { get; set; }

        // ---- navigational properties ----
        public virtual IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public virtual IEnumerable<Rating<Post>> PostsRatings { get; set; } = new HashSet<Rating<Post>>();
        public virtual IEnumerable<PostReply> PostReplies { get; set; } = new List<PostReply>();
        public virtual IEnumerable<Rating<PostReply>> PostReplyRatings { get; set; } = new HashSet<Rating<PostReply>>();
        // ---------------------------------
    }
}
