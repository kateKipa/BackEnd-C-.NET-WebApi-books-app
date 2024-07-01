
using BooksApiApp.Data;
using BooksApiApp.Model;
using BooksApiApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BooksApiApp.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : class, IEntity
    {
        protected readonly BooksWebApiContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(BooksWebApiContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);                      //Entity state changes to "Added"
        }

        public virtual async Task<int> AddAsyncWithReturnId(T entity)
        {
            await _dbSet.AddAsync(entity); 
            await _context.SaveChangesAsync();
            int generatedId = entity.Id;
            return generatedId;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);               //A list of entities is inserted in DbContext. Entity state of every entity changes to "Added"
        }                                                       //The insertion in the db will be done at the next SaveChangesAsync()

        public virtual async Task<bool> DeleteAsync(int id)
        {
            T? existing = await _dbSet.FindAsync(id);
            if (existing is not null)
            {
                _dbSet.Remove(existing);
                return true;
            }
            return false;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return entities;
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public virtual async Task<int> GetCountAsync()
        {
            var count = await _dbSet.CountAsync();
            return count;
        }

        public virtual void UpdateAsync(T entity)           //Attach: Entity is inserted into the DbContext (if it doesn't exist) & then entity state changes to "Modified".
        {                                                   //Update method does not need async version because does not communicate with the db.
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            //_dbSet.Update(entity);                        //Update even if the entity has not changed
        }
    }
}
