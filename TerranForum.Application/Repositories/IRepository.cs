using System.Linq.Expressions;
using TerranForum.Application.Utils;
using TerranForum.Domain.Interfaces;

namespace TerranForum.Application.Repositories
{
    public interface IRepository<TModel> where TModel : ISoftDeletableEntity
    {
        Task<bool> CreateAsync(TModel value);
        Task<TModel?> GetByIdAsync(int id, bool withDeleted = false);
        Task<TModel?> GetByIdWithDeletedAsync(int id);
        Task<IEnumerable<TModel>> GetAllAsync(Expression<Func<TModel, bool>>? predicate = null, bool withDeleted = false);
        Task<IEnumerable<TModel>> GetAllWithDeletedAsync(Expression<Func<TModel, bool>>? predicate = null);
        Task<IEnumerable<TModel>> GetAllOrderedAsync(Expression<Func<TModel, bool>>? predicate = null, Ordering<TModel>? ordering = null);
        Task<bool> DeleteAsync(TModel? value);
        Task<bool> UpdateAsync(TModel value);
        Task<bool> ExistsAsync(Expression<Func<TModel, bool>> predicate, bool withDeleted = false);
        Task<bool> ExistsWithDeletedAsync(Expression<Func<TModel, bool>> predicate);
        Task<bool> UndoDeleteAsync(TModel model);
    }
}
