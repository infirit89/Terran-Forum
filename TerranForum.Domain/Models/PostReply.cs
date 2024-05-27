using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Domain.Interfaces;

namespace TerranForum.Domain.Models
{
    public class PostReply : ILikeble, ISoftDeletableEntity
    {
        public int Id { get; set; }

        [Required,
        MinLength(Constants.MinPostReplyContentSize),
        MaxLength(Constants.MaxPostReplyContentSize)]
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public int PostId { get; set; }
        public virtual Post Post { get; set; } = null!;

        public virtual IEnumerable<Rating<PostReply>> Ratings { get; set; } = new List<Rating<PostReply>>();
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
