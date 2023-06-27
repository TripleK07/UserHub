using System;
using Microsoft.EntityFrameworkCore;
using UserHubAPI.Entities.Data;
using UserHubAPI.Repositories.IRepositories;

namespace UserHubAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();

        UserHubContext GetContext();

        IRepository<T> GetRepository<T>() where T : class;
    }
}