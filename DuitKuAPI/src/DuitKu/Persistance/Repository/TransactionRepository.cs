using Microsoft.EntityFrameworkCore;
using DuitKu.Domain;
using DuitKu.Persistance.Database;
using DuitKu.DTOs;

namespace DuitKu.Persistance.Repository
{
    public class TransactionRepository
    {
        private readonly ApplicationDBContext _context;
        // private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(
            ApplicationDBContext context
        // ILogger<AccountRepository> logger
        )
        {
            _context = context;
            // _logger = logger;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(Guid userId)
        {
            return await _context.Transactions
                .Where(transaction => transaction.UserId == userId)
                .Select(transaction => new Transaction
                {
                    Id = transaction.Id,
                    UserId = transaction.UserId,
                    AccountId = transaction.AccountId,
                    CategoryId = transaction.CategoryId,
                    SubCategoryId = transaction.SubCategoryId,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                    CreatedAt = transaction.CreatedAt,
                    UpdatedAt = transaction.UpdatedAt,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<TransactionsWithRelation>> GetAllWithRelationAsync(
            Guid userId,
            bool account,
            bool category,
            bool subcategory
        )
        {
            var query = _context.Transactions
                .Where(transaction => transaction.UserId == userId);

            if(account) {
                query.Include("Account");
            }

            if(category) {
                query.Include("Category");
            }

            if(subcategory) {
                query.Include("SubCategory");
            }

            return await query
                .Select(transaction => new TransactionsWithRelation
                {
                    Id = transaction.Id,
                    Category = category ? new Category {
                        Id = transaction.Category.Id,
                        Name = transaction.Category.Name,
                    } : null,
                    SubCategory = subcategory && transaction.SubCategoryId.HasValue ? new SubCategory {
                        Id = transaction.SubCategory.Id,
                        Name = transaction.SubCategory.Name,
                    } : null,
                    Account = account ? new Account {
                        Id = transaction.Account.Id,
                        Name = transaction.Account.Name,
                        CreatedAt = transaction.Account.CreatedAt,
                        UpdatedAt = transaction.Account.UpdatedAt,
                    } : null,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                    Date = transaction.Date
                })
                .ToListAsync();
        }

        public async Task<Transaction> GetByIdAsync(Guid transactionId, Guid userId)
        {
            return await _context.Transactions
                .Where(transaction => transaction.UserId == userId)
                .Where(transaction => transaction.Id == transactionId)
                .Select(transaction => new Transaction
                {
                    Id = transaction.Id,
                    UserId = transaction.UserId,
                    AccountId = transaction.AccountId,
                    CategoryId = transaction.CategoryId,
                    SubCategoryId = transaction.SubCategoryId,
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                    CreatedAt = transaction.CreatedAt,
                    UpdatedAt = transaction.UpdatedAt,
                })
                .FirstAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid transactionId, Guid userId)
        {
            var transaction = await _context.Transactions
                .Where(transaction => transaction.Id == transactionId)
                .Where(transaction => transaction.UserId == userId)
                .FirstAsync();

            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);

                await _context.SaveChangesAsync();
            }
        }
    }
}