using Microsoft.AspNetCore.Identity;
using TerranForum.Application.Services;
using TerranForum.Domain.Models;
using TerranForum.Domain.Enums;
using Jdenticon;
using System.Text;
using System.Security.Cryptography;
using TerranForum.Application.Dtos.UserDtos;
using TerranForum.Application.Repositories;
using TerranForum.Domain.Exceptions;
using TerranForum.Application.Responses;

namespace TerranForum.Infrastructure.Services
{
    internal class UserService : IUserService
    {
        public UserService(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IFileService fileService)
        {
            _UserManager = userManager;
            _UserStore = userStore;
            _FileService = fileService;
        }

        public async Task<bool> IsUserAdmin(string userId)
        {
            ApplicationUser user = await _UserManager.FindByIdAsync(userId);
            return await _UserManager.IsInRoleAsync(user, Roles.Admin);
        }

        public async Task<IdentityDataResponse<ApplicationUser>> CreateWithRoleAsync(CreateUserDto createUserDto)
        {
            ApplicationUser user = new ApplicationUser();
            if (createUserDto.CreateRandomProfilePicture) 
            {
                using (var hash = SHA256.Create())
                {
                    byte[] idHash = hash.ComputeHash(Encoding.UTF8.GetBytes(user.Id));
                    string iconFileName = $"{Guid.NewGuid()}.svg";
                    string iconFilePath = Path.Join(_FileService.UploadedImagesPath, iconFileName);
                    string relativeImageFilePath = Path.GetRelativePath(_FileService.ContentPath, iconFilePath);
                    await Identicon
                        .FromHash(idHash, 100)
                        .SaveAsSvgAsync(iconFilePath);

                    user.ProfileImageUrl = relativeImageFilePath;
                }
            }
            user.JoinedAt = DateTimeOffset.UtcNow;
            await _UserStore.SetUserNameAsync(user, createUserDto.Username, default);
            await((IUserEmailStore<ApplicationUser>)_UserStore).SetEmailAsync(user, createUserDto.Email, default);
            await((IUserEmailStore<ApplicationUser>)_UserStore).SetEmailConfirmedAsync(user, true, default);
            var response = new IdentityDataResponse<ApplicationUser>() { Data = null };
            response.Result = await _UserManager.CreateAsync(user, createUserDto.Password);
            if (!response.Result.Succeeded)
                return response;
            
            response.Result = await _UserManager.AddToRoleAsync(user, createUserDto.Role);
            if (!response.Result.Succeeded)
                return response;

            response.Data = user;
            return response;
        }

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IUserStore<ApplicationUser> _UserStore;
        private readonly IFileService _FileService;
    }
}
