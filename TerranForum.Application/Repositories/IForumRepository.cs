using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Repositories
{
    public interface IForumRepository : IRepository<Forum>
    {
        Task<GetForumPagedModel> GetForumsPagedAsync(int page, int size);
    }
}
