using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(bool IncludeDeleted =false); 
        Task<Category?> GetByIdAsync(int id, bool includeDeleted = false);
        Task<Category> AddAsync(Category category);
        Task<bool> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id); // baja lógica
    }
}