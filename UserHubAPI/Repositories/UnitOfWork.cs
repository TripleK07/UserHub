using System;
using Microsoft.EntityFrameworkCore;
using UserHubAPI.Entities.Data;
using UserHubAPI.Repositories.IRepositories;

namespace UserHubAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserHubContext _context;

        public UnitOfWork(UserHubContext context)
        {
            _context = context;
        }

        // Create repositories as needed, passing the DbContext to their constructors
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new BaseRepository<T>(_context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}