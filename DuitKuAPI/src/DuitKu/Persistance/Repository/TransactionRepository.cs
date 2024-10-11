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

        public async Task<int> GetTotalRecord(Guid userId)
        {
            return await _context.Transactions
                .Where(transaction => transaction.UserId == userId)
                .CountAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(Guid userId)
        {
            return await _context.Transactions
                .AsNoTracking()
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
            bool subcategory,
            int pageNumber
        )
        {
            int pageSize = 10;

            var query = _context.Transactions
                .AsNoTracking() // This data is for read only purpose, so no tracking needed
                .Where(transaction => transaction.UserId == userId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            if (account)
            {
                query.Include("Account");
            }

            if (category)
            {
                query.Include("Category");
            }

            if (subcategory)
            {
                query.Include("SubCategory");
            }

            return await query
                .Select(transaction => new TransactionsWithRelation
                {
                    Id = transaction.Id,
                    Category = category ? new Category
                    {
                        Id = transaction.Category.Id,
                        Name = transaction.Category.Name,
                    } : null,
                    SubCategory = subcategory && transaction.SubCategoryId.HasValue ? new SubCategory
                    {
                        Id = transaction.SubCategory.Id,
                        Name = transaction.SubCategory.Name,
                    } : null,
                    Account = account ? new Account
                    {
                        Id = transaction.Account.Id,
                        Name = transaction.Account.Name,
                        Balance = transaction.Account.Balance,
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
                .AsNoTracking()
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
            var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Account
                    .Where(account => account.Id == transaction.AccountId)
                    .ExecuteUpdateAsync(setter => setter
                            .SetProperty(account => account.Balance, account => account.Balance - transaction.Amount));

                _context.Transactions.Add(transaction);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(Transaction transaction, decimal oldAmount)
        {
            var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Account
                    .Where(account => account.Id == transaction.AccountId)
                    .ExecuteUpdateAsync(setter => setter
                            .SetProperty(account => account.Balance, account => (account.Balance + oldAmount) - transaction.Amount));

                _context.Transactions.Update(transaction);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(Guid transactionId, Guid userId)
        {
            var transaction = await _context.Transactions
                .Where(transaction => transaction.Id == transactionId)
                .Where(transaction => transaction.UserId == userId)
                .FirstAsync();

            if (transaction != null)
            {
                var dbTransaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    await _context.Account
                        .Where(account => account.Id == transaction.AccountId)
                        .ExecuteUpdateAsync(setter => setter
                                .SetProperty(account => account.Balance, account => account.Balance + transaction.Amount));

                    _context.Transactions.Remove(transaction);

                    await _context.SaveChangesAsync();
                    await dbTransaction.CommitAsync();
                }
                catch
                {
                    await dbTransaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}