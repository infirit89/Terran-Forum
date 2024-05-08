using System.Linq.Expressions;

namespace TerranForum.Application.Utils
{
    public class Ordering<T>
    {
        public static Ordering<T> OrderBy<TKey>(Expression<Func<T, TKey>> expression)
        {
            return new Ordering<T>(x => x.OrderBy(expression));
        }

        public Ordering<T> ThenBy<TKey>(Expression<Func<T, TKey>> expression) 
        {
            return new Ordering<T>(x => _Transform(x).ThenBy(expression));
        }

        public static Ordering<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> expression)
        {
            return new Ordering<T>(x => x.OrderByDescending(expression));
        }

        public Ordering<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> expression) 
        {
            return new Ordering<T>(x => _Transform(x).ThenByDescending(expression));
        }

        public IOrderedQueryable<T> Apply(IQueryable<T> queryable) => _Transform(queryable);

        private Ordering(Func<IQueryable<T>, IOrderedQueryable<T>> transform) 
        {
            _Transform = transform;
        }

        private readonly Func<IQueryable<T>, IOrderedQueryable<T>> _Transform;
    }
}
