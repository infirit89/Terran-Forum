using Microsoft.EntityFrameworkCore;
using TerranForum.Application.Repositories;
using TerranForum.Domain.Models;
using TerranForum.Infrastructure.Repositories;

namespace TerranForum.Tests.Repositories
{
    [TestFixture]
    public class ForumRepositoryTests
    {
        [SetUp]
        public void Initialize()
        {
            _ForumRepository = new ForumRepository(
                GlobalSetup.DbContext);
        }

        [Test]
        public async Task Create_ReturnsTrue()
        {
            Forum forum = new Forum
            {
                Title = "This is a test title"
            };

            Assert.That(
                await _ForumRepository.CreateAsync(forum),
                Is.True);
        }

        [Test]
        public async Task Destroy_ReturnsTrue()
        {
            Forum forum = await GlobalSetup
                .DbContext
                .Forums
                .FirstAsync(x => x.Title == "This is a test title");

            Assert.That(
                await _ForumRepository.DeleteAsync(forum),
                Is.True);
        }

        [Test]
        public async Task Destroy_ReturnsFalse()
        {
            Forum? forum = await GlobalSetup
                .DbContext
                .Forums
                .FirstOrDefaultAsync(x => x.Title == "Forum10000");

            Assert.That(
                await _ForumRepository.DeleteAsync(forum),
                Is.False);
        }

        [Test]
        public async Task GetAllAsync_ReturnsFourForums()
        {
            IEnumerable<Forum> forums = await _ForumRepository
                .GetAllAsync(x => x.Title.Contains("Forum"));

            Assert.That(forums.Count(), Is.EqualTo(4));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsForum0() 
        {
            Forum? forum = await _ForumRepository.GetByIdAsync(1);
            Assert.IsNotNull(forum);
            Assert.That(forum.Title, Is.EqualTo("Forum0"));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull()
        {
            Forum? forum = await _ForumRepository.GetByIdAsync(0);
            Assert.IsNull(forum);
        }

        [Test]
        public async Task ExistsAsync_ReturnsTrue()
        {
            Assert.That(await _ForumRepository
                .ExistsAsync(x => x.Title == "Forum0"),
                Is.True);
        }

        [Test]
        public async Task ExistsAsync_ReturnsFalse()
        {
            Assert.That(await _ForumRepository
                .ExistsAsync(x => x.Title == "Forum10000"),
                Is.False);
        }

        [Test]
        public async Task UpdateAsync_ReturnsTrue() 
        {
            Forum forum = await GlobalSetup
                .DbContext
                .Forums
                .FirstAsync(x => x.Title == "Forum0");

            forum.Title = "Forum10";
            Assert.That(
                await _ForumRepository.UpdateAsync(forum),
                Is.True);
        }

        private IForumRepository _ForumRepository;
    }
}
