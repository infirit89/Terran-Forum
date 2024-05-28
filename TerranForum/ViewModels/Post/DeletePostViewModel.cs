namespace TerranForum.ViewModels.Post
{
    public class DeletePostViewModel
    {
        public int PostId { get; set; }
        public int ForumId { get; set; }
        public bool IsMaster { get; set; }
    }
}
