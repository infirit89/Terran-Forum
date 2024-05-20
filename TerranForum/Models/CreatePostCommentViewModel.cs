using System.ComponentModel.DataAnnotations;

namespace TerranForum.Models
{
    public class CreatePostCommentViewModel
    {
        [Required]
        [Display(Name = "comment")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Content { get; set; } = null!;
        [Required]
        public int PostId { get; set; }
    }
}
