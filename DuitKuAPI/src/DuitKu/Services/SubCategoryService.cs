using DuitKu.Persistance.Repository;
using DuitKu.Domain;
using DuitKu.DTOs;

using Microsoft.EntityFrameworkCore;

namespace DuitKu.Services
{
    public class SubCategoryService(SubCategoryRepository repository, QueryService<SubCategory> queryService)
    {
        private readonly SubCategoryRepository _subCategoryRepository = repository;
        private readonly QueryService<SubCategory> _queryService = queryService;

        public async Task<int> GetTotalRecord(Guid userId)
        {
            return await _subCategoryRepository.GetTotalRecord(userId);
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategories(Guid userId, GetAllSubCategoriesFilterDto filterDto)
        {
            var query = _subCategoryRepository.GetEntities()
                .AsNoTracking()
                .Where(subCategory => subCategory.UserId == userId)
                .Where(subCategory => subCategory.CategoryId == filterDto.CategoryId);

            query = _queryService.PaginateWithSearchFilter(query, filterDto, (query, searchString) =>
            {
                string lowerCaseSearchString = searchString.ToLower();

                return query.Where(subCategory => EF.Functions.Like(subCategory.Name.ToLower(), $"%{lowerCaseSearchString}%"));
            });

            return await query.ToListAsync();
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