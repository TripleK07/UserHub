using System;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace UserHubAPI.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        EntityEntry<T> Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

