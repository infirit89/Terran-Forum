namespace TerranForum.ViewModels.User
{
    public class UserViewModel
    {
        public string Username { get; set; } = null!;
        public string ProfilePictureUrl { get; set; } = null!;
        public DateTimeOffset JoinedAt { get; set; }
    }
}
