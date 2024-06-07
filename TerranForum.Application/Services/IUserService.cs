using TerranForum.Application.Dtos.UserDtos;

namespace TerranForum.Application.Services
{
    public interface IUserService
    {
        Task<bool> IsUserAdmin(string userId);
        Task CreateWithRoleAsync(CreateUserDto createUserDto);
    }
}
