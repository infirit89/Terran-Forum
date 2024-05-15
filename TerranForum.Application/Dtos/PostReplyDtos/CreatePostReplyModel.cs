using System.ComponentModel.DataAnnotations;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Dtos.PostReplyDtos
{
    public class CreatePostReplyModel
    {
        [Required, MinLength(5)]
        public string Content { get; set; } = null!;
        [Required]
        public ApplicationUser User { get; set; } = null!;
        [Required]
        public Post Post { get; set; } = null!;
    }
}
