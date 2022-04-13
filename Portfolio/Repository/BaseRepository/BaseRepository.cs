using Abstraction.Interfaces.Repositories;
using Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Repository.BaseRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        private readonly AppDbContext _dbContext;

        public BaseRepository(AppDbContext db)
        {
            _dbContext = db;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            EntityEntry<T> entry = _dbContext.Attach(entity);
            entry.State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }
       
    }
}
