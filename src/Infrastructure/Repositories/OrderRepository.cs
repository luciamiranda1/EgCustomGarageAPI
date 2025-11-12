using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationContext _context;

        public OrderRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<User?> GetClientByIdAsync(int clientId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == clientId);
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // === Órdenes por producto ===
        public async Task<List<Order>> GetOrdersByProductIdAsync(int productId)
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Product)
                .Where(o => o.ProductId == productId)
                .OrderByDescending(o => o.Date)
                .ToListAsync();
        }
    }
}
