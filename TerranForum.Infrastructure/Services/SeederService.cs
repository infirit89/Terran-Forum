using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Application.Services;
using TerranForum.Domain.Enums;
using TerranForum.Domain.Models;

namespace TerranForum.Infrastructure.Services
{
    public class SeederService : ISeederService
    {
        public SeederService(ILogger<SeederService> logger,
            RoleManager<IdentityRole> roleManager, 
            IUserStore<ApplicationUser> userStore, 
            UserManager<ApplicationUser> userManager,
            TerranForumDbContext dbContext)
        {
            _Logger = logger;
            _RoleManager = roleManager;
            _UserStore = userStore;
            _UserManager = userManager;
            _DbContext = dbContext;
        }

        public async Task SeedRolesAsync()
        {
            string[] roles = { Roles.Admin, Roles.User };
            _Logger.LogInformation("Seeding roles: {0}", string.Join(", ", roles));

            foreach (var role in roles)
            {
                if (!await _RoleManager.RoleExistsAsync(role))
                    await _RoleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public async Task SeedUsersAsync()
        {
            _Logger.LogInformation("Seeding users:");
            await CreateUserAndAddToRole(TestAdmin, Roles.Admin);
            await CreateUserAndAddToRole(TestUser, Roles.User);
        }

        public async Task SeedForumAsync()
        {
            _Logger.LogInformation("Seeding forum");
            Forum? forum = await _DbContext.Forums.FirstOrDefaultAsync(x => x.Title == TestForum);
            if (forum == null)
            {
                forum = new Forum()
                {
                    Title = TestForum
                };
                await _DbContext.Forums.AddAsync(forum);
            }

            if (!await _DbContext.Posts.AnyAsync(x => x.Content == TestForumMasterPost)) 
            {
                ApplicationUser user = await _UserManager.FindByNameAsync(TestUser);
                Post masterPost = new Post()
                {
                    Content = TestForumMasterPost,
                    User = user,
                    CreatedAt = DateTime.Now,
                    Forum = forum,
                    IsMaster = true
                };

                await _DbContext.Posts.AddAsync(masterPost);
            }

            await _DbContext.SaveChangesAsync();
        }

        private async Task<bool> CreateUserAndAddToRole(string userName, string role) 
        {
            _Logger.LogInformation("\tCreating user: {0} with role: {1}", userName, role);
            ApplicationUser user = new ApplicationUser();
            await _UserStore.SetUserNameAsync(user, userName, default);
            await ((IUserEmailStore<ApplicationUser>)_UserStore).SetEmailAsync(user, userName, default);
            await ((IUserEmailStore<ApplicationUser>)_UserStore).SetEmailConfirmedAsync(user, true, default);
            var result = await _UserManager.CreateAsync(user, TestPassword);
            if (result != IdentityResult.Success)
                return false;

            await _UserManager.AddToRoleAsync(user, role);

            return true;
        }

        private readonly ILogger<SeederService> _Logger;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IUserStore<ApplicationUser> _UserStore;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly TerranForumDbContext _DbContext;
        private const string TestAdmin = "Admin0";
        private const string TestUser = "User0";
        private const string TestPassword = "Test@T1";
        private const string TestForum = "Forum0";
        private const string TestForumMasterPost = "Hello, this is a test post!";
    }
}
