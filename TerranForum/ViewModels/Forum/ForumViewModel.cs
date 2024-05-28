using TerranForum.Domain.Models;

namespace TerranForum.ViewModels.Forum
{
    public class ForumViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Rating { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string CreatorUserName { get; set; } = null!;
    }
}
