using TerranForum.ViewModels.PostReply;

namespace TerranForum.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int ForumId { get; set; }
        public string CreatorUserName { get; set; } = null!;
        public int Rating { get; set; }
        public short CurrentUserRating { get; set; }
        public string Content { get; set; } = null!;
        public bool IsMaster { get; set; }
        public IEnumerable<PostReplyViewModel> Replies { get; set; } = null!;
    }
}
