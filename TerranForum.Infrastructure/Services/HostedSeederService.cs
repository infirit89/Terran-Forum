using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TerranForum.Application.Services;

namespace TerranForum.Infrastructure.Services
{
    public class HostedSeederService : IHostedService
    {
        public HostedSeederService(IServiceProvider serviceProvider) 
        {
            _ServiceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _ServiceProvider.CreateAsyncScope()) 
            {
                var seederService = scope.ServiceProvider.GetRequiredService<ISeederService>();
                await seederService.SeedRolesAsync();
                await seederService.SeedUsersAsync();
                await seederService.SeedForumAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private readonly IServiceProvider _ServiceProvider;
    }
}
