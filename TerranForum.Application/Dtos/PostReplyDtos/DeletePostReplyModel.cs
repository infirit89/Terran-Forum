namespace TerranForum.Application.Dtos.PostReplyDtos
{
    public class DeletePostReplyModel
    {
        public int ReplyId { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; } = null!;
    }
}
