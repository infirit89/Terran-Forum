using TerranForum.Domain.Models;

namespace TerranForum.Models
{
    public class ForumViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Rating { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
