using System.ComponentModel.DataAnnotations;

namespace TerranForum.Models
{
    public class CreatePostCommentViewModel
    {
        [Required, MinLength(5)]
        public string Content { get; set; } = null!;
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public int PostId { get; set; }
    }
}
