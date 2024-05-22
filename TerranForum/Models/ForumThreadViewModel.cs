using TerranForum.Domain.Models;

namespace TerranForum.Models
{
    public class ForumThreadViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
