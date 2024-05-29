namespace TerranForum.Application.Dtos.ForumDtos
{
    public class UpdateForumModel
    {
        public int ForumId { get; set; }
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
