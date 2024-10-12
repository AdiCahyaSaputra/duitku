using DuitKu.Domain;
using DuitKu.DTOs;
using DuitKu.Persistance.Repository;

namespace DuitKu.Services
{
    public class TransactionService
    {
        private readonly TransactionRepository _transactionRepository;

        public TransactionService(TransactionRepository repository)
        {
            _transactionRepository = repository;
        }

        public async Task<int> GetTotalRecord(Guid userId)
        {
            return await _transactionRepository.GetTotalRecord(userId);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions(Guid userId)
        {
            return await _transactionRepository.GetAllAsync(userId);
        }

        public async Task<IEnumerable<TransactionsWithRelation>> GetAllTransactionsWith(
            Guid userId,
            bool account,
            bool category,
            bool subcategory,
            BaseParamFilterDto filterDto)
        {
            return await _transactionRepository.GetAllWithRelationAsync(userId, account, category, subcategory, filterDto);
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