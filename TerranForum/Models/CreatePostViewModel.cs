using System.ComponentModel.DataAnnotations;
using TerranForum.Domain.Models;

namespace TerranForum.Models
{
    public class CreatePostViewModel
    {
        [Required]
        [Display(Name = "post")]
        [StringLength(
        Constants.MaxPostContentSize, 
        ErrorMessage = ErrorMessages.CreatePostErrorMessage,
        MinimumLength = Constants.MinPostContentSize)]
        public string Content { get; set; } = null!;
        [Required]
        public int ForumId { get; set; }
    }
}
