using DuitKu.Persistance.Repository;
using DuitKu.Domain;
using DuitKu.DTOs;

namespace DuitKu.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService(CategoryRepository repository)
        {
            _categoryRepository = repository;
        }

        public async Task<IEnumerable<Category>> GetAllCategories(Guid userId)
        {
            var categories = await _categoryRepository.GetAllAsync(userId);

            return categories;
        }

        public async Task<Category> GetById(Guid categoryId, Guid userId)
        {
            return await _categoryRepository.GetByIdAsync(categoryId, userId);
        }

        public async Task<Category> CreateCategory(CategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                UserId = dto.UserId,
            };

            await _categoryRepository.AddAsync(category);

            return category;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            await _categoryRepository.UpdateAsync(category);

            return category;
        }

        public async Task DeleteCategory(Guid categoryId, Guid userId)
        {
            await _categoryRepository.DeleteAsync(categoryId, userId);
        }
    }
}