namespace TerranForum.Application.Dtos.PostDtos
{
    public class DeletePostModel
    {
        public int PostId { get; set; }
        public int ForumId { get; set; }
        public string UserId { get; set; } = null!;
    }
}
