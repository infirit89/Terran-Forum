using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Application.Utils;
using TerranForum.Domain.Models;

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
