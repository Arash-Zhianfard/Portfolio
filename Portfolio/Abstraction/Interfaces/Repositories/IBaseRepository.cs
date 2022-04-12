using Abstraction.Models;

namespace Abstraction.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetListAsync();
        Task<T> GetAsync(int id);
    }
}
