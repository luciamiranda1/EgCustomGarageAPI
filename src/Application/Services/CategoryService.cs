using Application.Models;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(bool includeDeleted = false)
        {
            return await _categoryRepository.GetAllAsync(includeDeleted);
        }

        public async Task<Category?> GetByIdAsync(int id, bool includeDeleted = false)
        {
            return await _categoryRepository.GetByIdAsync(id, includeDeleted);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            return await _categoryRepository.AddAsync(category);
        }
        public async Task<bool> UpdateAsync(Category category)
        {
            return await _categoryRepository.UpdateAsync(category);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }
    }
}