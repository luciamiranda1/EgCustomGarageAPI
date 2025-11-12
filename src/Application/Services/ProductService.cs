using Application.Models.Response;
using Application.Models.Request;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(bool includeDeleted = false)
        {
            var products = await _productRepository.GetAllAsync(includeDeleted);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                CategoryName = p.Category?.Name
            });
        }

        public async Task<ProductDto?> GetByIdAsync(int id, bool includeDeleted = false)
        {
            var p = await _productRepository.GetByIdAsync(id, includeDeleted);
            if (p == null) return null;

            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                CategoryName = p.Category?.Name
            };
        }

        public async Task<ProductDto?> CreateAsync(CreateProductRequest req)
        {
            var product = new Product
            {
                Name = req.Name,
                Price = req.Price,
                Stock = req.Stock,
                CategoryId = req.CategoryId
            };

            var created = await _productRepository.AddAsync(product);
            var loaded = await _productRepository.GetByIdAsync(created.Id);

            if (loaded == null) return null;

            return new ProductDto
            {
                Id = loaded.Id,
                Name = loaded.Name,
                Price = loaded.Price,
                Stock = loaded.Stock,
                CategoryName = loaded.Category?.Name
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductRequest req)
        {
            var p = await _productRepository.GetByIdAsync(id);
            if (p == null || p.IsDeleted) return false;

            p.Name = req.Name;
            p.Price = req.Price;
            p.Stock = req.Stock;
            p.CategoryId = req.CategoryId;

            return await _productRepository.UpdateAsync(p);
        }

        public async Task<bool> DeleteAsync(int id)
            => await _productRepository.DeleteAsync(id);
    }
}
