using Application.Models.Response;  
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Crea la orden con IDs y cantidad (sin usar DTO en el service)
        public async Task<OrderResponseDTO> CreateOrderAsync(int clientId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("La cantidad debe ser mayor a 0.");

            var product = await _orderRepository.GetProductByIdAsync(productId);
            var client  = await _orderRepository.GetClientByIdAsync(clientId);

            if (product is null)
                throw new Exception("El producto no existe.");
            if (client is null)
                throw new Exception("El cliente no existe.");

            if (product.Stock < quantity)
                throw new Exception("No hay stock suficiente para este producto.");

            product.Stock -= quantity;
            await _orderRepository.UpdateProductAsync(product);

            var order = new Order
            {
                ClientId  = clientId,
                ProductId = productId,
                Quantity  = quantity,
                Date      = DateTime.UtcNow
            };

            var saved = await _orderRepository.CreateAsync(order);

            return new OrderResponseDTO
            {
                Id          = saved.Id,
                ProductId   = saved.ProductId,
                ClientId    = saved.ClientId,
                Quantity    = saved.Quantity,
                Date        = saved.Date,
                ProductName = product.Name,
                ClientName  = client.Name,
                Total = saved.Quantity * product.Price
            };
        }

        // Listar órdenes por producto
        public async Task<List<OrderResponseDTO>> GetClientsByProductIdAsync(int productId)
        {
            var orders = await _orderRepository.GetOrdersByProductIdAsync(productId);

            return orders.Select(o => new OrderResponseDTO
            {
                Id          = o.Id,
                ProductId   = o.ProductId,
                ClientId    = o.ClientId,
                Quantity    = o.Quantity,
                Date        = o.Date,
                ProductName = o.Product?.Name,
                ClientName  = o.Client?.Name,
                Total = o.Quantity * (o.Product?.Price ?? 0)
            }).ToList();
        }
    }
}
