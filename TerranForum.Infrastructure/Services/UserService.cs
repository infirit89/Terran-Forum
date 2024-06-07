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

namespace TerranForum.Infrastructure.Services
{
    internal class UserService : IUserService
    {
        public UserService(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IFileService fileService,
            IUserRepository userRepository)
        {
            _UserManager = userManager;
            _UserStore = userStore;
            _FileService = fileService;
            _UserRepository = userRepository;
        }

        public async Task<bool> IsUserAdmin(string userId)
        {
            ApplicationUser user = await _UserManager.FindByIdAsync(userId);
            return await _UserManager.IsInRoleAsync(user, Roles.Admin);
        }

        public async Task CreateWithRoleAsync(CreateUserDto createUserDto)
        {
            if (await _UserRepository.ExsistsAsync(x => x.Email == createUserDto.Email))
                throw new ModelAlreadyExistsException();
            
            ApplicationUser user = new ApplicationUser();
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
            await _UserStore.SetUserNameAsync(user, createUserDto.Username, default);
            await((IUserEmailStore<ApplicationUser>)_UserStore).SetEmailAsync(user, createUserDto.Email, default);
            await((IUserEmailStore<ApplicationUser>)_UserStore).SetEmailConfirmedAsync(user, true, default);
            var result = await _UserManager.CreateAsync(user, createUserDto.Password);
            if (!result.Succeeded)
                throw new CreateModelException();

            await _UserManager.AddToRoleAsync(user, createUserDto.Role);
        }

        private readonly IUserRepository _UserRepository;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IUserStore<ApplicationUser> _UserStore;
        private readonly IFileService _FileService;
    }
}
