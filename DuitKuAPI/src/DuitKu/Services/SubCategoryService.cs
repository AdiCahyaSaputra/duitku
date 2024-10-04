using DuitKu.Persistance.Repository;
using DuitKu.Persistance.Database;
using DuitKu.Domain;
using DuitKu.DTOs;

namespace DuitKu.Services
{
    public class SubCategoryService
    {
        private readonly SubCategoryRepository _subCategoryRepository;

        public SubCategoryService(SubCategoryRepository repository)
        {
            _subCategoryRepository = repository;
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategories(Guid userId)
        {
            var subCategories = await _subCategoryRepository.GetAllAsync(userId);

            return subCategories;
        }

        public async Task<SubCategory> GetById(Guid subCategoryId, Guid userId)
        {
            return await _subCategoryRepository.GetByIdAsync(subCategoryId, userId);
        }

        public async Task<SubCategory> CreateSubCategory(SubCategoryDto dto)
        {
            var subCategory = new SubCategory
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                UserId = dto.UserId,
            };

            await _subCategoryRepository.AddAsync(subCategory);

            return subCategory;
        }

        public async Task<SubCategory> UpdateSubCategory(SubCategory subCategory)
        {
            await _subCategoryRepository.UpdateAsync(subCategory);

            return subCategory;
        }

        public async Task DeleteSubCategory(Guid accountId, Guid userId)
        {
            await _subCategoryRepository.DeleteAsync(accountId, userId);
        }
    }
}