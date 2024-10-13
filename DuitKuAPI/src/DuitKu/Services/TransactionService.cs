using DuitKu.Domain;
using DuitKu.DTOs;
using DuitKu.Persistance.Repository;

using Microsoft.EntityFrameworkCore;

namespace DuitKu.Services
{
    public class TransactionService
    {
        private readonly TransactionRepository _transactionRepository;
        private readonly QueryService<Transaction> _queryService;

        public TransactionService(
            TransactionRepository repository,
            QueryService<Transaction> queryService)
        {
            _transactionRepository = repository;
            _queryService = queryService;
        }

        public async Task<int> GetTotalRecord(Guid userId)
        {
            return await _transactionRepository.GetTotalRecord(userId);
        }

        public async Task<IEnumerable<TransactionsWithRelation>> GetAllTransactionsWith(
            Guid userId,
            BaseParamFilterDto filterDto)
        {
            var query = _transactionRepository.GetEntities()
                .AsNoTracking()
                .Where(transaction => transaction.UserId == userId);

            query = _queryService.PaginateWithSearchFilter(query, filterDto, (query, searchString) =>
            {
                string lowerCaseSearchString = searchString.ToLower();

                return query.Where(transaction => EF.Functions.Like(transaction.Description.ToLower(), $"%{lowerCaseSearchString}%"));
            });

            query = query.Include("Account").Include("Category").Include("SubCategory");

            return await query
                .Select(transaction => new TransactionsWithRelation
                {
                    Id = transaction.Id,
                    Category = new Category
                    {
                        Id = transaction.Category.Id,
                        Name = transaction.Category.Name,
                    },
                    SubCategory = transaction.SubCategoryId.HasValue ? new SubCategory
                    {
                        Id = transaction.SubCategory.Id,
                        Name = transaction.SubCategory.Name,
                    } : null,
                    Account = new Account
                    {
                        Id = transaction.Account.Id,
                        Name = transaction.Account.Name,
                        Balance = transaction.Account.Balance,
                        CreatedAt = transaction.Account.CreatedAt,
                        UpdatedAt = transaction.Account.UpdatedAt,
                    },
                    Description = transaction.Description,
                    Amount = transaction.Amount,
                    Date = transaction.Date
                })
                .ToListAsync();
        }

        public async Task<Transaction> GetById(Guid transactionId, Guid userId)
        {
            return await _transactionRepository.GetByIdAsync(transactionId, userId);
        }

        public async Task<Transaction> CreateTransaction(TransactionDto dto)
        {
            var transaction = new Transaction
            {
                Description = dto.Description,
                Amount = dto.Amount,
                AccountId = dto.AccountId,
                CategoryId = dto.CategoryId,
                SubCategoryId = dto.SubCategoryId,
                UserId = dto.UserId,
                Date = dto.Date ?? DateTime.UtcNow,
            };

            await _transactionRepository.AddAsync(transaction);

            return transaction;
        }

        public async Task<Transaction> UpdateTransaction(Transaction transaction, decimal oldAmount)
        {
            await _transactionRepository.UpdateAsync(transaction, oldAmount);

            return transaction;
        }

        public async Task DeleteTransaction(Guid transactionId, Guid userId)
        {
            await _transactionRepository.DeleteAsync(transactionId, userId);
        }
    }
}