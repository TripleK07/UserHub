using System;
namespace UserHubAPI.Services
{
	public interface IService<T> where T : class
	{
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);
        Task<Entities.Users> Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<bool> Exists(Guid id);
    }
}

