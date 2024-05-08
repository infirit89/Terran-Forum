using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Dtos.ForumDtos
{
    public class GetForumPagedModel
    {
        public IEnumerable<Forum> Forums { get; set; }
        public int PageCount { get; set; }
    }
}
