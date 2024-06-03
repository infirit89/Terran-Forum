using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Application.Repositories;
using TerranForum.Domain.Models;
using TerranForum.Infrastructure.Repositories;

namespace TerranForum.Tests.Repositories
{
    [TestFixture]
    public class PostRepositoryTests
    {

        [SetUp]
        public void Initialize() 
        {
            _PostRepository = new PostRepository(GlobalSetup.DbContext);
        }

        //[Test]
        //public async Task Create_ReturnsTrue()
        //{
        //    ApplicationUser user = await GlobalSetup
        //        .DbContext.Users
        //        .FirstAsync();

        //    Post post = new Post
        //    {
        //        Content = "This is a test post hello",
        //        User = user,
        //        CreatedAt = DateTime.Now
        //    };
        //}

        private IPostRepository _PostRepository;
    }
}
