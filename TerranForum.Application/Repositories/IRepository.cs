using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerranForum.Domain.Models;

namespace TerranForum.Application.Repositories
{
    public interface IRepository<TModel> where TModel : class
    {
        Task<bool> CreateAsync(TModel value);
        Task<TModel?> GetByIdAsync(int id);
        Task<IEnumerable<TModel>> GetAllAsync(Predicate<TModel>? predicate = null);
        Task<bool> DeleteAsync(TModel value);
        Task<bool> UpdateAsync(TModel value);
        Task<bool> ExsistsAsync(Predicate<TModel> predicate);
    }
}
