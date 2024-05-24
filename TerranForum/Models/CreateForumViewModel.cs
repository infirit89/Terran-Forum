using System.ComponentModel.DataAnnotations;
using TerranForum.Domain.Models;

namespace TerranForum.Models
{
    public class CreateForumViewModel
    {
        [Required]
        [Display(Name = "title")]
        [StringLength(
        maximumLength: Constants.MaxForumThreadTitleSize,
        ErrorMessage = ErrorMessages.CreateErrorMessage,
        MinimumLength = Constants.MinForumThreadTitleSize)]
        public string Title { get; set; } = null!;

        [Required]
        [Display(Name = "content")]
        [StringLength(
        Constants.MaxPostContentSize,
        ErrorMessage = ErrorMessages.CreateErrorMessage,
        MinimumLength = Constants.MinPostContentSize)]
        public string Content { get; set; } = null!;
    }
}
