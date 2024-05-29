using TerranForum.ViewModels.Post;

namespace TerranForum.ViewModels.Forum
{
    public class ForumThreadViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public IEnumerable<PostViewModel> Posts { get; set; } = null!;
    }
}
