using TerranForum.Application.Repositories;
using TerranForum.Infrastructure.Repositories;

namespace TerranForum.Tests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        [SetUp]
        public void Initialize()
        {
            _UserRepository = new UserRepository(GlobalSetup.DbContext);
        }

        [Test]
        public async Task ExistsById_ReturnsTrue()
        {
            Assert.That(await _UserRepository.ExsistsAsync(x => x.Id == "3800c45c-ae61-4a82-be70-7fb1e8af6aef"), Is.True);
        }

        [Test]
        public async Task ExistsById_ReturnsFalse()
        {
            Assert.That(await _UserRepository.ExsistsAsync(x => x.Id == "592bdc52-4745-41be-9214-b3031f28cfcf"), Is.False);
        }

        private IUserRepository _UserRepository;
    }
}
