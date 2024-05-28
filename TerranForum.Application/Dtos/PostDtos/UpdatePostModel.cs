using System.ComponentModel.DataAnnotations;

namespace TerranForum.Application.Dtos.PostDtos
{
    public class UpdatePostModel
    {
        public int PostId { get; set; }
        public string UserId { get; set; } = null!;
        public string PostContent { get; set; } = null!;
    }
}
