using System.Linq.Expressions;
using TerranForum.Application.Utils;

namespace TerranForum.Application.Repositories
{
    public interface IRepository<TModel> where TModel : class
    {
        Task<bool> CreateAsync(TModel value);
        Task<TModel?> GetByIdAsync(int id);
        Task<IEnumerable<TModel>> GetAllAsync(Expression<Func<TModel, bool>>? predicate = null, Ordering<TModel>? ordering = null);
        Task<bool> DeleteAsync(TModel value);
        Task<bool> UpdateAsync(TModel value);
        Task<bool> ExsistsAsync(Expression<Func<TModel, bool>> predicate);
    }
}
