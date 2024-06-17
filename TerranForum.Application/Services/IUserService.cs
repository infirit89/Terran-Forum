using TerranForum.Application.Dtos.UserDtos;
using TerranForum.Application.Responses;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Services
{
    public interface IUserService
    {
        Task<bool> IsUserAdminAsync(string userId);
        Task<IdentityDataResponse<ApplicationUser>> CreateWithRoleAsync(CreateUserDto createUserDto);
    }
}
