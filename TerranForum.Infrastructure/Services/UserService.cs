using Microsoft.AspNetCore.Identity;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;
using TerranForum.Domain.Enums;

namespace TerranForum.Infrastructure.Services
{
    internal class UserService : IUserService
    {
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;
        }

        public async Task<bool> IsUserAdmin(string userId)
        {
            ApplicationUser user = await _UserManager.FindByIdAsync(userId);
            return await _UserManager.IsInRoleAsync(user, Roles.Admin);
        }

        private readonly UserManager<ApplicationUser> _UserManager;
    }
}
