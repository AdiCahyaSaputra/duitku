using DuitKu.Domain;
using DuitKu.DTOs;
using DuitKu.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace DuitKu.Persistance.Repository
{
    public class AccountRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(
            ApplicationDBContext context,
            ILogger<AccountRepository> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<Account> GetEntities() 
        {
            return _context.Account;
        }

        public async Task<int> GetTotalRecord(Guid userId)
        {
            return await _context.Account
                .Where(account => account.UserId == userId)
                .CountAsync();
        }

        public async Task<IEnumerable<AccountWithTransactionsDto>> GetAllWithTransactionAsync(Guid userId)
        {
            return await _context.Account
                .Where(account => account.UserId == userId)
                .Include(account => account.Transactions)
                .Select(account => new AccountWithTransactionsDto
                {
                    Id = account.Id,
                    Name = account.Name,
                    Balance = account.Balance,
                    Transactions = account.Transactions,
                })
                .ToListAsync();
        }

        public async Task<Account> GetByIdAsync(Guid accountId, Guid userId)
        {
            return await _context.Account
                .Where(account => account.UserId == userId)
                .Where(account => account.Id == accountId)
                .Select(account => new Account
                {
                    Id = account.Id,
                    Name = account.Name,
                    Balance = account.Balance,
                    CreatedAt = account.CreatedAt,
                    UpdatedAt = account.UpdatedAt,
                })
                .FirstAsync();
        }

        public async Task AddAsync(Account account)
        {
            _context.Account.Add(account);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Account.Update(account);

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid accountId, Guid userId)
        {
            var account = await _context.Account
                .Where(account => account.UserId == userId)
                .Where(account => account.Id == accountId)
                .FirstAsync();

            if (account != null)
            {
                _context.Account.Remove(account);

                await _context.SaveChangesAsync();

                return 1;
            }

            return 0;
        }
    }
}