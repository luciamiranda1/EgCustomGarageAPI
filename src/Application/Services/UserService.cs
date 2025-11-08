using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        // === GET por nombre ===
        public async Task<UserModel?> GetAsync(string name, bool includeDeleted = false)
        {
            var user = await _repository.GetByNameAsync(name, includeDeleted);
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = string.Empty, // no exponemos password
                Role = user.Role
            };
        }

        // === GET listado ===
        public async Task<List<UserModel>> GetAsync(bool includeDeleted = false)
        {
            var users = await _repository.GetAllAsync(includeDeleted);
            return users.Select(u => new UserModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Password = string.Empty, // no exponemos password
                Role = u.Role
            }).ToList();
        }

        // === GET por Id ===
        public async Task<UserModel?> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var user = await _repository.GetByIdAsync(id, includeDeleted);
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = string.Empty, // no exponemos password
                Role = user.Role
            };
        }

        // === UPDATE ===
        public async Task UpdateAsync(int id, UserForUpdateRequest request)
        {
            var existingUser = await _repository.GetByIdAsync(id, includeDeleted: true);
            if (existingUser == null) return;

            existingUser.Name = request.Name;
            existingUser.Email = request.Email;
            existingUser.Password = request.Password;
            existingUser.Role = request.Role;

            await _repository.UpdateAsync(existingUser);
        }

        // === ADD ===
        public async Task<int> AddUserAsync(UserForAddRequest request)
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };

            var created = await _repository.AddAsync(user);
            return created.Id; // devolvemos el Id como en tu versión
        }

        // === LOGIN simple (por nombre) ===
        public async Task<UserModel?> CheckCredentialsAsync(CredentialsRequest credentials)
        {
            var user = await _repository.GetByNameAsync(credentials.Name, includeDeleted: false);
            if (user != null && user.Password == credentials.Password)
            {
                return new UserModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Password = string.Empty // no exponemos password
                };
            }
            return null;
        }

        // === DELETE (baja lógica) ===
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
