using TerranForum.Domain.Models;

namespace TerranForum.Models
{
    public class ForumThreadViewModel
    {
        public string Title { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
