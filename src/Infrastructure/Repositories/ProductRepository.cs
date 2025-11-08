using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationContext _context;
        public ProductRepository(ApplicationContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(bool includeDeleted = false)
        {
            var q = _context.Products
            .Include(p => p.Category)
            .AsQueryable(); //consulta que se puede seguir modificando (no se ejecuta todavía).
            if (!includeDeleted) //Verifica si el usuario pidió incluir eliminados
                q = q.Where(p => !p.IsDeleted); //condición para traer solo los activos
            return await q.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var q = _context.Products
            .Include(p => p.Category).
            AsQueryable();
            if (!includeDeleted) //false
                q = q.Where(p => !p.IsDeleted); //NO eliminado
            return await q.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product; //devuele el producto creadoo + id generado en bd
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            _context.Products.Update(product); 
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _context.Products
            .FindAsync(id);
            if (p is null) return false;
            p.IsDeleted = true; // baja lógica
            _context.Products.Update(p);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
