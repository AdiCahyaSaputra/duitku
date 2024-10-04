using DuitKu.Persistance.Repository;
using DuitKu.Persistance.Database;
using DuitKu.Domain;
using DuitKu.DTOs;

namespace DuitKu.Services
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;

        public AccountService(AccountRepository repository)
        {
            _accountRepository = repository;
        }

        public async Task<IEnumerable<Account>> GetAllAccount(Guid userId)
        {
            var accounts = await _accountRepository.GetAllAsync(userId);

            return accounts;
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