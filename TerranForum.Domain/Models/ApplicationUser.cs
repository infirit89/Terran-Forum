using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TerranForum.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public virtual IEnumerable<Rating<Post>> PostsRatings { get; set; } = new List<Rating<Post>>();
        public virtual IEnumerable<PostReply> PostReplies { get; set; } = new List<PostReply>();
        public virtual IEnumerable<Rating<PostReply>> PostReplyRatings { get; set; } = new List<Rating<PostReply>>();
    }
}
