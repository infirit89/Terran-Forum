using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Application.Repositories;
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
            IPostRepository postRepository,
            IForumRepository forumRepository)
        {
            _Logger = logger;
            _RoleManager = roleManager;
            _UserStore = userStore;
            _UserManager = userManager;
            _PostRepository = postRepository;
            _ForumRepository = forumRepository;
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

            for (int i = 0; i < _TestForums.Count; i++)
            {
                TestForumData testForumData = _TestForums[i];

                Forum? forum = await _ForumRepository.GetByIdAsync(testForumData.Id);
                if (forum == null) 
                {
                    forum = new Forum()
                    {
                        Title = testForumData.Title
                    };
                    await _ForumRepository.CreateAsync(forum);
                }

                TestForumPostData masterPostData = _TestForumPosts[i];
                TestForumPostData replyPostData = _TestForumPosts[i + 1];
                if (!await _PostRepository.ExsistsAsync(x => x.Id == masterPostData.Id))
                {
                    ApplicationUser user = await _UserManager.FindByNameAsync(TestUser);
                    Post masterPost = new Post()
                    {
                        Content = masterPostData.Content,
                        User = user,
                        CreatedAt = DateTime.Now,
                        Forum = forum,
                        IsMaster = true
                    };

                    await _PostRepository.CreateAsync(masterPost);
                }

                if (!await _PostRepository.ExsistsAsync(x => x.Id == replyPostData.Id))
                {
                    ApplicationUser user = await _UserManager.FindByNameAsync(TestUser);
                    Post post = new Post()
                    {
                        Content = replyPostData.Content,
                        User = user,
                        CreatedAt = DateTime.Now,
                        Forum = forum,
                        IsMaster = false
                    };

                    await _PostRepository.CreateAsync(post);
                }
            }
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
        private readonly IPostRepository _PostRepository;
        private readonly IForumRepository _ForumRepository;
        private const string TestAdmin = "Admin0@mail.com";
        private const string TestUser = "User0@mail.com";
        private const string TestPassword = "Test@T1";
        
        private struct TestForumData 
        {
            public string Title;
            public int Id;
        }

        private struct TestForumPostData 
        {
            public string Content;
            public int Id;
        }

        private List<TestForumData> _TestForums = new List<TestForumData>() 
        {
            new TestForumData() 
            {
                Title = "Forum0",
                Id = 1
            },
            new TestForumData() 
            {
                Title = "Forum1",
                Id = 2
            }
        };

        private List<TestForumPostData> _TestForumPosts = new List<TestForumPostData>()
        {
            new TestForumPostData() 
            {
                Content = "Hello, this is a test post!",
                Id = 1
            },
            new TestForumPostData()
            {
                Content = "This is a test reply",
                Id = 2
            },
            new TestForumPostData()
            {
                Content = "Hello, this is a second test post!",
                Id = 3
            },
            new TestForumPostData()
            {
                Content = "This is a second test reply",
                Id = 4
            },
        };
    }
}
