using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerranForum.Domain.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        [Range(0, uint.MaxValue)]
        public uint UpvoteCount { get; set; } = 0;

        [Range(0, uint.MaxValue)]
        public uint DownvoteCount { get; set; } = 0;
        public virtual IEnumerable<PostReply> Replies { get; set; } = new List<PostReply>();
        [Required]
        public int ForumId { get; set; }
        public virtual Forum Forum { get; set; } = null!;

        public bool IsMaster { get; init; }
    }
}
