using Jdenticon;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Application.Dtos.PostDtos;
using TerranForum.Application.Dtos.PostReplyDtos;
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
            IForumRepository forumRepository,
            IForumService forumService,
            IPostService postService,
            IPostReplyService postReplyService,
            IFileService fileService,
            IUserRepository userRepository)
        {
            _Logger = logger;
            _RoleManager = roleManager;
            _UserStore = userStore;
            _UserManager = userManager;
            _PostRepository = postRepository;
            _ForumRepository = forumRepository;
            _ForumService = forumService;
            _PostService = postService;
            _PostReplyService = postReplyService;
            _FileService = fileService;
            _UserRepository = userRepository;
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
                TestForumPostData masterPostData = _TestForumPosts[i * 3];
                TestForumPostData answerPostData = _TestForumPosts[i * 3 + 1];
                TestForumPostData replyPostData = _TestForumPosts[i * 3 + 2];
                ApplicationUser user = await _UserManager.FindByNameAsync(TestUser);

                Forum? forum = await _ForumRepository.GetByIdWithDeletedAsync(testForumData.Id);

                if (forum == null)
                {
                    CreateForumModel createForumModel = new()
                    {
                        Title = testForumData.Title,
                        Content = masterPostData.Content,
                        UserId = user.Id
                    };
                    forum = await _ForumService.CreateForumThreadAsync(createForumModel);
                    if (forum == null)
                    {
                        _Logger.LogCritical("Couldn't seed forum");
                        return;
                    }

                    Post masterPost = forum.Posts.First();
                    CreatePostReplyModel createPostReplyModel = new()
                    {
                        Content = replyPostData.Content,
                        UserId = user.Id,
                        PostId = masterPost.Id
                    };
                    await _PostReplyService.AddPostReply(createPostReplyModel);
                }
                else 
                {
                    await _ForumRepository.UndoDeleteAsync(forum);
                    Post? masterPost = await _PostRepository.GetFirstWithAsync(x => x.ForumId == forum.Id && x.IsMaster);
                    await _PostRepository.UndoDeleteAsync(masterPost);
                }

                Post? post = await _PostRepository.GetByIdWithDeletedAsync(answerPostData.Id);

                if (post == null)
                {
                    CreatePostModel createPostModel = new()
                    {
                        Content = answerPostData.Content,
                        UserId = user.Id,
                        ForumId = forum!.Id
                    };

                    await _PostService.AddPostToThread(createPostModel);
                }
                else 
                {
                    await _PostRepository.UndoDeleteAsync(post);
                }
            }
        }

        private async Task<bool> CreateUserAndAddToRole(string userName, string role)
        {
            if (await _UserRepository.ExsistsAsync(x => x.UserName == userName)) 
            {
                _Logger.LogInformation("\tUser: {0} with role: {1} already exists", userName, role);
                return false;
            }

            _Logger.LogInformation("\tCreating user: {0} with role: {1}", userName, role);
            ApplicationUser user = new ApplicationUser();
            using (var hash = SHA256.Create())
            {
                byte[] idHash = hash.ComputeHash(Encoding.UTF8.GetBytes(user.Id));
                string iconFileName = $"{Guid.NewGuid()}.svg";
                string iconFilePath = Path.Join(_FileService.UploadedImagesPath, iconFileName);
                await Identicon
                    .FromHash(idHash, 100)
                    .SaveAsSvgAsync(iconFilePath);

                user.ProfileImageUrl = iconFilePath;
            }
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
        private readonly IForumService _ForumService;
        private readonly IPostService _PostService;
        private readonly IPostReplyService _PostReplyService;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IUserStore<ApplicationUser> _UserStore;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IPostRepository _PostRepository;
        private readonly IForumRepository _ForumRepository;
        private readonly IUserRepository _UserRepository;
        private const string TestAdmin = "Admin0@mail.com";
        private const string TestUser = "User0@mail.com";
        private const string TestPassword = "Test@T1";
        private readonly IFileService _FileService;
        
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
                Content = "This is a test answer",
                Id = 2
            },
            new TestForumPostData()
            {
                Content = "This is a test reply",
                Id = 1
            },
            new TestForumPostData()
            {
                Content = "Hello, this is a second test post!",
                Id = 3
            },
            new TestForumPostData()
            {
                Content = "This is a second test answer",
                Id = 4
            },
            new TestForumPostData() 
            {
                Content = "This is a second test reply",
                Id = 2
            }
        };
    }
}
