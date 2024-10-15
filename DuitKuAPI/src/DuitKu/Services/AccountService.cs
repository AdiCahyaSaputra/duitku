using DuitKu.Persistance.Repository;
using DuitKu.Domain;
using DuitKu.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DuitKu.Services
{
    public class AccountService(AccountRepository repository, QueryService<Account> queryService)
    {
        private readonly AccountRepository _accountRepository = repository;
        private readonly QueryService<Account> _queryService = queryService;

        public async Task<int> GetTotalRecord(Guid userId)
        {
            return await _accountRepository.GetTotalRecord(userId);
        }

        public async Task<IEnumerable<Account>> GetAllAccount(Guid userId, BaseParamFilterDto filterDto)
        {
            var query = _accountRepository.GetEntities()
                .AsNoTracking()
                .Where(account => account.UserId == userId);

            query = _queryService.PaginateWithSearchFilter(query, filterDto, (query, searchString) =>
            {
                string lowerCaseSearchString = searchString.ToLower();

                return query.Where(account => EF.Functions.Like(account.Name.ToLower(), $"%{lowerCaseSearchString}%"));
            });

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<AccountWithTransactionsDto>> GetAllAccountWithTransactions(Guid userId)
        {
            return await _accountRepository.GetAllWithTransactionAsync(userId);
        }

        public async Task<Account> GetById(Guid accountId, Guid userId)
        {
            return await _accountRepository.GetByIdAsync(accountId, userId);
        }

        public async Task<Account> CreateAccount(AccountDto dto)
        {
            var account = new Account
            {
                Name = dto.Name,
                Balance = dto.Balance,
                UserId = dto.UserId,
            };

            await _accountRepository.AddAsync(account);

            return account;
        }

        public async Task<Account> UpdateAccount(Account account)
        {
            await _accountRepository.UpdateAsync(account);

            return account;
        }

        public async Task DeleteAccount(Guid accountId, Guid userId)
        {
            await _accountRepository.DeleteAsync(accountId, userId);
        }
    }
}