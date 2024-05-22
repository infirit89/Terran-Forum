using System.ComponentModel.DataAnnotations;
using TerranForum.Domain.Models;

namespace TerranForum.Models
{
    public class CreatePostCommentViewModel
    {
        [Required]
        [Display(Name = "comment")]
        [StringLength(
        Constants.MaxPostReplyContentSize,
        ErrorMessage = ErrorMessages.CreatePostCommentErrorMessage,
        MinimumLength = Constants.MinPostReplyContentSize)]
        public string Content { get; set; } = null!;

        [Required]
        public int PostId { get; set; }
        [Required]
        public int ForumId { get; set; }
    }
}
