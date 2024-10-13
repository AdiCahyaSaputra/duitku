using DuitKu.Domain;
using DuitKu.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace DuitKu.Persistance.Repository
{
    public class CategoryRepository
    {
        private readonly ApplicationDBContext _context;

        public CategoryRepository(
            ApplicationDBContext context
        )
        {
            _context = context;
        }

        public IQueryable<Category> GetEntities() 
        {
            return _context.Category;
        }

        public async Task<int> GetTotalRecord(Guid userId)
        {
            return await _context.Category
                .Where(category => category.UserId == userId)
                .CountAsync();
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