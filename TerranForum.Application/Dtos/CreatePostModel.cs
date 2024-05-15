using System.ComponentModel.DataAnnotations;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Dtos
{
    public class CreatePostModel
    {
        [Required, MinLength(5)]
        public string Content { get; set; } = null!;
        [Required]
        public Forum Forum { get; set; } = null!;
        [Required]
        public ApplicationUser User { get; set; } = null!;
    }
}
