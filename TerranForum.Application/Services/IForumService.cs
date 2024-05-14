using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Application.Dtos.ForumDtos;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Services
{
    public interface IForumService
    {
        Task<Forum?> CreateForumThreadAsync(CreateForumModel createForumModel);
    }
}
