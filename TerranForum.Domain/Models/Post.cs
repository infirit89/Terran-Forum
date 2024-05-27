using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Domain.Interfaces;

namespace TerranForum.Domain.Models
{
    public class Post : ILikeble, ISoftDeletableEntity
    {
        public int Id { get; set; }

        [Required,
        MinLength(Constants.MinPostContentSize),
        MaxLength(Constants.MaxPostContentSize)]
        public string Content { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public virtual IEnumerable<PostReply> Replies { get; set; } = new List<PostReply>();
        [Required]
        public int ForumId { get; set; }
        public Forum Forum { get; set; } = null!;

        public bool IsMaster { get; init; }
        public virtual IEnumerable<Rating<Post>> Ratings { get; set; } = new List<Rating<Post>>();
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
