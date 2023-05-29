using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UserHubAPI.Entities.Data;
using UserHubAPI.Repositories.IRepositories;

namespace UserHubAPI.Repositories
{
	public class BaseRepository<T> : IRepository<T> where T : class
	{
        private readonly UserHubContext _context;
        private readonly DbSet<T> _set;

        public BaseRepository(UserHubContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            T? entity = await _set.FindAsync(id);
            if(entity !=null)
                _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public EntityEntry<T> Add(T entity)
        {
            //to prevent adding GUID by user/swagger
            _context.Entry(entity).Property("ID").CurrentValue = Guid.Empty;
            _context.Entry(entity).Property("CreatedBy").CurrentValue = "Admin";
            _context.Entry(entity).Property("ModifiedBy").CurrentValue = "Admin";
            return _set.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).Property("CreatedBy").CurrentValue = "Admin";
            _context.Entry(entity).Property("ModifiedDate").CurrentValue = DateTime.Now;
            _context.Entry(entity).Property("ModifiedBy").CurrentValue = "Admin";
            _set.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _set.Remove(entity);
        }
    }
}