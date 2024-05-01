using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerranForum.Application.Services
{
    public interface ISeederService
    {
        Task SeedRolesAsync();
        Task SeedUsersAsync();
        Task SeedForumAsync();
    }
}
