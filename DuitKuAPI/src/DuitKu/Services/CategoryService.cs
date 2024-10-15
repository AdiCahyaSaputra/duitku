using DuitKu.Persistance.Repository;
using DuitKu.Domain;
using DuitKu.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DuitKu.Services
{
    public class CategoryService(CategoryRepository repository, QueryService<Category> queryService)
    {
        private readonly CategoryRepository _categoryRepository = repository;
        private readonly QueryService<Category> _queryService = queryService;

        public async Task<int> GetTotalRecord(Guid userId)
        {
            return await _categoryRepository.GetTotalRecord(userId);
        }

        public async Task<IEnumerable<Category>> GetAllCategories(Guid userId, BaseParamFilterDto filterDto)
        {
            var query = _categoryRepository.GetEntities()
                .AsNoTracking()
                .Where(category => category.UserId == userId);

            query = _queryService.PaginateWithSearchFilter(query, filterDto, (query, searchString) =>
            {
                string lowerCaseSearchString = searchString.ToLower();

                return query.Where(categoory => EF.Functions.Like(categoory.Name.ToLower(), $"%{lowerCaseSearchString}%"));
            });

            return await query.ToListAsync();
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