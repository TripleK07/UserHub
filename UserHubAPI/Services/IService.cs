using System;
namespace UserHubAPI.Services
{
	public interface IService<T> where T : class
	{
        Task<IEnumerable<T>> GetAll();
        Task<T?> GetById(Guid id);
        Task<T> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(Guid id);
        Task<bool> Exists(Guid id);
    }
}