using Microsoft.EntityFrameworkCore;
using DuitKu.Domain;
using DuitKu.Persistance.Database;

namespace DuitKu.Persistance.Repository
{
    public class TransactionRepository
    {
        private readonly ApplicationDBContext _context;

        public TransactionRepository(
            ApplicationDBContext context
        )
        {
            _context = context;
        }

        public IQueryable<Transaction> GetEntities() 
        {
            return _context.Transactions;
        }

        public async Task<int> GetTotalRecord(Guid userId)
        {
            return await _context.Transactions
                .Where(transaction => transaction.UserId == userId)
                .CountAsync();
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