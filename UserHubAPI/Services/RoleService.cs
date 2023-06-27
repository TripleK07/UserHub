using System;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using UserHubAPI.Entities;
using UserHubAPI.Entities.Data;
using UserHubAPI.Repositories;
using UserHubAPI.Repositories.IRepositories;

namespace UserHubAPI.Services
{
    public interface IRoleService : IService<Roles>
    {
        // Additional methods specific to the entity
        Task<Roles?> GetRoleByRoleName(String userName);
        Task<List<Roles>> GetAllIncludeMenu();
        Task<Roles?> GetByIdIncludeMenu(Guid Id);
    }

    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Roles> _roleRepository;
        private readonly IRepository<RoleMenu> _roleMenuRepository;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = _unitOfWork.GetRepository<Roles>();
            _roleMenuRepository = _unitOfWork.GetRepository<RoleMenu>();
        }

        public async Task<IEnumerable<Roles>> GetAll()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<Roles?> GetById(Guid id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<Roles> Create(Roles role)
        {
            var entity = _roleRepository.Add(role);
            await _unitOfWork.CommitAsync();
            return await Task.FromResult(entity.Entity as Roles);
        }

        public async Task<bool> Update(Roles role)
        {
            var dbUser = await GetById(role.ID);
            if (dbUser == null)
            {
                return false;
            }
            else
            {
                UserHubContext context = _unitOfWork.GetContext();
                var roleMenus = context.RoleMenu.Where(rm => rm.RoleId == role.ID);
                context.RoleMenu.RemoveRange(roleMenus);

                foreach (RoleMenu rm in role.RoleMenu)
                {
                    _roleMenuRepository.Add(rm);
                }

                _roleRepository.Update(role);
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
                _roleRepository.Delete(dbUser);
                await _unitOfWork.CommitAsync();
                return true;
            }
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _roleRepository.GetByIdAsync(id) != null;
        }
        public async Task<Roles?> GetRoleByRoleName(String roleName)
        {
            return await Task.FromResult(_roleRepository.GetAllAsync().Result
                                        .FirstOrDefault(x => x.RoleName == roleName));
        }

        public Task<List<Roles>> GetAllIncludeMenu()
        {
            UserHubContext _context = _unitOfWork.GetContext();
            return _context.Roles.Include(r=> r.RoleMenu).ThenInclude(rm=> rm.Menu).ToListAsync();
        }

        public Task<Roles?> GetByIdIncludeMenu(Guid Id)
        {
            UserHubContext _context = _unitOfWork.GetContext();
            return Task.FromResult(_context.Roles.Include(r=> r.RoleMenu).ThenInclude(rm=> rm.Menu).FirstOrDefault(r=> r.ID == Id));
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}