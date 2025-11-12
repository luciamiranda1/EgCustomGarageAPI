using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrderRepository
    {
        // Crear una nueva orden
        Task<Order> CreateAsync(Order order);

        // Obtener producto por Id (para validar existencia o stock)
        Task<Product?> GetProductByIdAsync(int productId);

        // Obtener cliente por Id, obtener del JWT).
        Task<User?> GetClientByIdAsync(int clientId);

        // Actualizar datos del producto (por ejemplo, stock)
        Task UpdateProductAsync(Product product);

        // Obtener todas las órdenes de un producto específico
        Task<List<Order>> GetOrdersByProductIdAsync(int productId);
    }
}
