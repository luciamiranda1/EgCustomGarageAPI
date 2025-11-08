using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        // Obtener todos los usuarios (con opción a incluir los eliminados)
        Task<IEnumerable<User>> GetAllAsync(bool includeDeleted = false);

        // Obtener usuario por Id
        Task<User?> GetByIdAsync(int id, bool includeDeleted = false);

        Task<User?> GetByNameAsync(string name, bool includeDeleted = false);

        // Agregar un nuevo usuario
        Task<User> AddAsync(User user);

        // Actualizar un usuario existente
        Task<bool> UpdateAsync(User user);

        // Baja lógica
        Task<bool> DeleteAsync(int id);
    }
}
