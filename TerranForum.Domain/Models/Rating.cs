using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Domain.Interfaces;

namespace TerranForum.Domain.Models
{
    public class Rating<T> : ISoftDeletableEntity
        where T : ILikeble
    {
        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        [Required]
        public int ServiceId { get; set; }
        public T Service { get; set; }

        [Range(-1, 1)]
        public sbyte Value { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
