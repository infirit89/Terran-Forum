using System.ComponentModel.DataAnnotations;

namespace TerranForum.Application.Dtos.PostDtos
{
    public class UpdatePostRatingModel
    {
        [Range(-1, 1)]
        public sbyte Rating { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public int PostId { get; set; }
    }
}
