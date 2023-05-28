using System;
using UserHubAPI.Entities;
using UserHubAPI.Repositories;
using UserHubAPI.Repositories.IRepositories;

namespace UserHubAPI.Services
{
    public interface IUserService : IService<Users>
    {
        // Additional methods specific to the User entity
        Task<IEnumerable<Users>> GetActiveUsers();
        Task<IEnumerable<Users>> GetUsersByRole(string roleName);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Users> _userRepository;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<Users>();
        }

        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<Users> GetById(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<Users> Create(Users user)
        {
            var entity = _userRepository.Add(user);
            await _unitOfWork.CommitAsync();
            return await Task.FromResult(entity.Entity as Users);
        }

        public async Task<bool> Update(Users user)
        {
            var dbUser = await GetById(user.ID);
            if (dbUser == null)
                return false;
            else
            {
                _userRepository.Update(user);
                await _unitOfWork.CommitAsync();
                return true;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            var dbUser = await GetById(id);
            if (dbUser == null)
                return false;
            else
            {
                _userRepository.Delete(dbUser);
                await _unitOfWork.CommitAsync();
                return true;
            }
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _userRepository.GetByIdAsync(id) != null;
        }

        public async Task<IEnumerable<Users>> GetActiveUsers()
        {
            return null;
        }

        public async Task<IEnumerable<Users>> GetUsersByRole(string roleName)
        {
            return null;
            // Custom implementation for getting users by role
            //return await _userRepository.GetUsersByRoleAsync(roleName);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }

}

