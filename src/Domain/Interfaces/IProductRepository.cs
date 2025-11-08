using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(bool includeDeleted = false);
        Task<Product?> GetByIdAsync(int id, bool includeDeleted = false);
        Task<Product> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id); // baja lógica
    }
}
