using System.ComponentModel.DataAnnotations;

namespace TerranForum.Models
{
    public class CreatePostViewModel
    {
        [Required]
        [Display(Name = "comment")]
        [StringLength(400, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        public string Content { get; set; } = null!;
        [Required]
        public int ForumId { get; set; }
    }
}
