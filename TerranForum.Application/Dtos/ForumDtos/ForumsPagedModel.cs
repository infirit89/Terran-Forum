using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Dtos.ForumDtos
{
    public class ForumsPagedModel
    {
        public IEnumerable<Forum> Data { get; set; } = null!;
        public int PageCount { get; set; }
    }
}
