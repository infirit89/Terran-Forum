namespace TerranForum.ViewModels.Forum
{
    public class ForumThreadViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public IEnumerable<Domain.Models.Post> Posts { get; set; } = null!;
    }
}
