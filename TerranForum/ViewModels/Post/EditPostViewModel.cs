using System.ComponentModel.DataAnnotations;
using TerranForum.Domain.Models;

namespace TerranForum.ViewModels.Post
{
    public class EditPostViewModel
    {
        [Required]
        [Display(Name = "post")]
        [StringLength(
        Constants.MaxPostContentSize,
        ErrorMessage = ErrorMessages.CreateErrorMessage,
        MinimumLength = Constants.MinPostContentSize)]
        public string Content { get; set; } = null!;

        public int PostId { get; set; }
        public int ForumId { get; set; }
    }
}
