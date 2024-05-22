namespace TerranForum.Application.Services
{
    public interface ISeederService
    {
        Task SeedRolesAsync();
        Task SeedUsersAsync();
        Task SeedForumAsync();
    }
}
