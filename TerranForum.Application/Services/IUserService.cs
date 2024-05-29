namespace TerranForum.Application.Services
{
    public interface IUserService
    {
        Task<bool> IsUserAdmin(string userId);
    }
}
