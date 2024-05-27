﻿namespace TerranForum.Domain.Interfaces
{
    public interface ISoftDeletableEntity
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public void Undo()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
