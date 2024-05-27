using System.ComponentModel.DataAnnotations;
using TerranForum.Domain.Interfaces;

namespace TerranForum.Domain.Models
{
    public class Forum : ISoftDeletableEntity
    {
        public int Id { get; set; }

        [Required,
        MinLength(Constants.MinForumThreadTitleSize),
        MaxLength(Constants.MaxForumThreadTitleSize)]
        public string Title { get; set; } = null!;
        public virtual IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
