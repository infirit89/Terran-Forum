﻿namespace TerranForum.Application.Dtos.UserDtos
{
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool CreateRandomProfilePicture { get; set; } = true;
    }
}
