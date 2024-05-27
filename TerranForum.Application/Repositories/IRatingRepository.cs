using System.Linq.Expressions;
using TerranForum.Domain.Interfaces;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Repositories
{
    public interface IRatingRepository<TModel> where TModel : ILikeble
    {
        Task<bool> CreateAsync(Rating<TModel> model);
        Task<bool> ExistsAsync(Expression<Func<Rating<TModel>, bool>> predicate);
        Task<bool> UpdateAsync(Rating<TModel> model);
        Task<Rating<TModel>?> GetAsync(string userId, int serviceId);
    }
}
