using DuitKu.Domain;
using DuitKu.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace DuitKu.Persistance.Repository
{
    public class CategoryRepository
    {
        private readonly ApplicationDBContext _context;
        // private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(
            ApplicationDBContext context
        // ILogger<AccountRepository> logger
        )
        {
            _context = context;
            // _logger = logger;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(Guid userId)
        {
            return await _context.Category
                .Where(category => category.UserId == userId)
                .Select(category => new Category
                {
                    Id = category.Id,
                    Name = category.Name,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                })
                .ToListAsync();
        }

        public async Task<Category> GetByIdAsync(Guid categoryId, Guid userId)
        {
            return await _context.Category
                .Where(category => category.UserId == userId)
                .Where(category => category.Id == categoryId)
                .Select(category => new Category
                {
                    Id = category.Id,
                    Name = category.Name,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                })
                .FirstAsync();
        }

        public async Task AddAsync(Category category)
        {
            _context.Category.Add(category);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Category.Update(category);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid categoryId, Guid userId)
        {
            var category = await _context.Category
                .Where(category => category.UserId == userId)
                .Where(category => category.Id == categoryId)
                .FirstAsync();

            if (category != null)
            {
                _context.Category.Remove(category);

                await _context.SaveChangesAsync();
            }
        }
    }

}