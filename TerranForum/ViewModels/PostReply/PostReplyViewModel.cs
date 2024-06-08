namespace TerranForum.ViewModels.PostReply
{
    public class PostReplyViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
    }
}
