using System.Linq.Expressions;
using SocialNetwork.Entities.Base;

namespace SocialNetwork.Repository;

public interface IRepository<T> where T : IBaseEntity
{
    Task<T> GetAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync(int offset = 0, int limit = 10);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, int offset = 0, int limit = 10);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}