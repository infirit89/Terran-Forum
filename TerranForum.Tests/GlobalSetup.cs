﻿using Microsoft.EntityFrameworkCore;
using TerranForum.Domain.Models;
using TerranForum.Infrastructure;
using TerranForum.Infrastructure.Services;

namespace TerranForum.Tests
{
    [SetUpFixture]
    public class GlobalSetup
    {
        [OneTimeSetUp]
        public async Task Initialize() 
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "3800c45c-ae61-4a82-be70-7fb1e8af6aef",
                UserName = "test@mail.com",
                NormalizedUserName = "TEST@MAIL.COM",
                Email = "test@mail.com",
                NormalizedEmail = "TEST@MAIL.COM",
                PasswordHash = "sometesthash",
            };
            var options = new DbContextOptionsBuilder<TerranForumDbContext>()
            .UseInMemoryDatabase("TerranForumInMemory")
            .Options;

            DbContext = new TerranForumDbContext(options, new SoftDeleteInterceptor());

            await DbContext.AddAsync(user);

            List<Forum> forums = new List<Forum>()
            {
                new Forum() { Title = "Forum0" },
                new Forum() { Title = "Forum1" },
                new Forum() { Title = "Forum2" },
                new Forum() { Title = "Forum3" },
            };

            await DbContext.AddRangeAsync(forums);

            await DbContext.SaveChangesAsync();
        }

        public static TerranForumDbContext DbContext { get; private set; }
    }
}
