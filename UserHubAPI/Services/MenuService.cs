using System;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UserHubAPI.Entities;
using UserHubAPI.Helper;
using UserHubAPI.Repositories;
using UserHubAPI.Repositories.IRepositories;

namespace UserHubAPI.Services
{
    public interface IMenuService : IService<Menus>
    {
        // Additional methods specific to the entity
        Task<List<Menus>> GetMenusByUser(String loginID);
    }

    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Menus> _menuRepository;

        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _menuRepository = _unitOfWork.GetRepository<Menus>();
        }

        public async Task<IEnumerable<Menus>> GetAll()
        {
            return await _menuRepository.GetAllAsync();
        }

        public async Task<Menus?> GetById(Guid id)
        {
            return await _menuRepository.GetByIdAsync(id);
        }

        public async Task<Menus> Create(Menus role)
        {
            var entity = _menuRepository.Add(role);
            await _unitOfWork.CommitAsync();
            return await Task.FromResult(entity.Entity as Menus);
        }

        public async Task<bool> Update(Menus role)
        {
            var dbUser = await GetById(role.ID);
            if (dbUser == null)
            {
                return false;
            }
            else
            {
                _menuRepository.Update(role);
                await _unitOfWork.CommitAsync();
                return true;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            var dbUser = await GetById(id);
            if (dbUser == null)
            {
                return false;
            }
            else
            {
                _menuRepository.Delete(dbUser);
                await _unitOfWork.CommitAsync();
                return true;
            }
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _menuRepository.GetByIdAsync(id) != null;
        }

        public async Task<List<Menus>> GetMenusByUser(String loginID)
        {
            const string query = @"select m.*
                            from Users as u
                            inner join UserRole as ur on ur.UserId = u.ID
                            inner join Roles as r on ur.RoleId = r.ID
                            inner join RoleMenu as rm on rm.RoleId = r.ID
                            inner join Menus as m on rm.MenuId = m.ID
                            where u.LoginID = @loginID
                            and u.IsActive = 1
                            and r.IsActive = 1
                            and m.IsActive = 1";
            var _context = _unitOfWork.GetContext();
            return await _context.Menus
                                .FromSqlRaw(query, new SqlParameter("@loginID", loginID))
                                .ToListAsync() ?? new List<Menus>();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}