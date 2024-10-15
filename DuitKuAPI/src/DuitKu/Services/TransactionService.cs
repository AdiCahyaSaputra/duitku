using DuitKu.Domain;
using DuitKu.DTOs;
using DuitKu.Persistance.Repository;

using Microsoft.EntityFrameworkCore;

namespace DuitKu.Services
{
    public class TransactionService(
        TransactionRepository repository,
        QueryService<Transaction> queryService)
    {
        private readonly TransactionRepository _transactionRepository = repository;
        private readonly QueryService<Transaction> _queryService = queryService;

        public async Task<int> GetTotalRecord(Guid userId)
        {
            return await _transactionRepository.GetTotalRecord(userId);
        }

        public async Task<IEnumerable<TransactionsWithRelation>> GetAllTransactionsWith(
            Guid userId,
            FilterTransactionDto filterDto)
        {
            var query = _transactionRepository.GetEntities()
                .AsNoTracking()
                .Where(transaction => transaction.UserId == userId)
                .Include("Account")
                .Include("Category")
                .Include("SubCategory");

            query = _queryService.PaginateWithSearchFilter(
                query,
                filterDto,
                (query, searchString) => query.Where(transaction => EF.Functions.Like(transaction.Description.ToLower(), $"%{searchString.ToLower()}%")));

            query = _queryService.When(query, filterDto.AccountId.HasValue, (query) => query.Where(transaction => transaction.AccountId == filterDto.AccountId));
            query = _queryService.When(query, filterDto.CategoryId.HasValue, (query) => query.Where(transaction => transaction.CategoryId == filterDto.CategoryId));
            query = _queryService.When(query, filterDto.SubCategoryId.HasValue, (query) => query.Where(transaction => transaction.SubCategoryId == filterDto.SubCategoryId));
            query = _queryService.When(query, filterDto.DateStart.HasValue, (query) => query.Where(transaction => DateTime.Compare(transaction.Date.Date, ((DateTime)filterDto.DateStart!).Date) >= 0));
            query = _queryService.When(query, filterDto.DateEnd.HasValue, (query) => query.Where(transaction => DateTime.Compare(transaction.Date.Date, ((DateTime)filterDto.DateEnd!).Date) <= 0));

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

        public async Task<decimal> GetTotalExpense(Guid userId, TransactionExpenseFilterDto expenseFilterDto)
        {
            var query = _transactionRepository.GetEntities()
                .AsNoTracking()
                .Where(transaction => transaction.UserId == userId);

            query = _queryService.When(query, expenseFilterDto.AccountId.HasValue, (query) => query.Where(transaction => transaction.AccountId == expenseFilterDto.AccountId));
            query = _queryService.When(query, expenseFilterDto.DateStart.HasValue, (query) => query.Where(transaction => DateTime.Compare(transaction.Date.Date, ((DateTime)expenseFilterDto.DateStart!).Date) >= 0));
            query = _queryService.When(query, expenseFilterDto.DateEnd.HasValue, (query) => query.Where(transaction => DateTime.Compare(transaction.Date.Date, ((DateTime)expenseFilterDto.DateEnd!).Date) <= 0));

            var transactions = await query.ToListAsync();
            decimal totalExpense = 0;

            transactions.ForEach(transaction => totalExpense += transaction.Amount);

            return totalExpense;
        }

        public async Task<IEnumerable<MostExpensiveTransactionDto>> GetExpensiveTransaction(Guid userId, TransactionExpenseFilterDto expenseFilterDto)
        {
            var query = _transactionRepository.GetEntities()
                .AsNoTracking()
                .Where(transaction => transaction.UserId == userId)
                .Include("Account");

            query = _queryService.When(query, expenseFilterDto.DateStart.HasValue, (query) => query.Where(transaction => DateTime.Compare(transaction.Date.Date, ((DateTime)expenseFilterDto.DateStart!).Date) >= 0));
            query = _queryService.When(query, expenseFilterDto.DateEnd.HasValue, (query) => query.Where(transaction => DateTime.Compare(transaction.Date.Date, ((DateTime)expenseFilterDto.DateEnd!).Date) <= 0));

            return await query
                .OrderByDescending(transaction => transaction.Amount)
                .Take(3)
                .Select(transaction => new MostExpensiveTransactionDto
                {
                    Id = transaction.Id,
                    Account = new Account
                    {
                        Id = transaction.Account.Id,
                        Name = transaction.Account.Name,
                    },
                    Amount = transaction.Amount,
                    Description = transaction.Description,
                    Date = transaction.Date,
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