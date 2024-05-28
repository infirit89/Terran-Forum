using System.ComponentModel.DataAnnotations;
using TerranForum.Domain.Models;

namespace TerranForum.ViewModels.Post
{
    public class CreatePostViewModel
    {
        [Required]
        [Display(Name = "post")]
        [StringLength(
        Constants.MaxPostContentSize,
        ErrorMessage = ErrorMessages.CreateErrorMessage,
        MinimumLength = Constants.MinPostContentSize)]
        public string Content { get; set; } = null!;
        [Required]
        public int ForumId { get; set; }
    }
}
