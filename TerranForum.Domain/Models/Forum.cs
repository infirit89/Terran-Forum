using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerranForum.Domain.Models
{
    public class Forum
    {
        public int Id { get; set; }
        [Required, MinLength(5)]
        public string Title { get; set; } = null!;
        public virtual IEnumerable<Post> Posts { get; set; } = new List<Post>();
    }
}
