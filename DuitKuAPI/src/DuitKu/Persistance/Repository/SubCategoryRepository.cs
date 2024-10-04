using DuitKu.Domain;
using DuitKu.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace DuitKu.Persistance.Repository
{
    public class SubCategoryRepository
    {
        private readonly ApplicationDBContext _context;
        // private readonly ILogger<CategoryRepository> _logger;

        public SubCategoryRepository(
            ApplicationDBContext context
        // ILogger<AccountRepository> logger
        )
        {
            _context = context;
            // _logger = logger;
        }

        public async Task<IEnumerable<SubCategory>> GetAllAsync(Guid userId)
        {
            return await _context.SubCategory
                .Where(subCategory => subCategory.UserId == userId)
                .Select(subCategory => new SubCategory
                {
                    Id = subCategory.Id,
                    CategoryId = subCategory.CategoryId,
                    Name = subCategory.Name,
                    CreatedAt = subCategory.CreatedAt,
                    UpdatedAt = subCategory.UpdatedAt,
                })
                .ToListAsync();
        }

        public async Task<SubCategory> GetByIdAsync(Guid subCategoryId, Guid userId)
        {
            return await _context.SubCategory
                .Where(subCategory => subCategory.UserId == userId)
                .Where(subCategory => subCategory.Id == subCategoryId)
                .Select(subCategory => new SubCategory
                {
                    Id = subCategory.Id,
                    CategoryId = subCategory.CategoryId,
                    Name = subCategory.Name,
                    CreatedAt = subCategory.CreatedAt,
                    UpdatedAt = subCategory.UpdatedAt,
                })
                .FirstAsync();
        }

        public async Task AddAsync(SubCategory subCategory)
        {
            _context.SubCategory.Add(subCategory);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SubCategory subCategory)
        {
            _context.SubCategory.Update(subCategory);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid subCategoryId, Guid userId)
        {
            var subCategory = await _context.SubCategory
                .Where(subCategory => subCategory.UserId == userId)
                .Where(subCategory => subCategory.Id == subCategoryId)
                .FirstAsync();

            if (subCategory != null)
            {
                _context.SubCategory.Remove(subCategory);

                await _context.SaveChangesAsync();
            }
        }
    }
}