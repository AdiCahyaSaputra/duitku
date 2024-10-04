using Microsoft.EntityFrameworkCore;
using DuitKu.Domain;
using DuitKu.Persistance.Database;

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