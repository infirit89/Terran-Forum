using System.ComponentModel.DataAnnotations;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Dtos
{
    public class CreatePostModel
    {
        [Required, MinLength(5)]
        public string Content { get; set; } = null!;
        [Required]
        public int ForumId { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
    }
}
