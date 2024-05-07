using System.ComponentModel.DataAnnotations;

namespace TerranForum.Application.Dtos.Forum
{
    public class CreateForumModel
    {
        [Required, MinLength(5)]
        public string Title { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
    }
}
