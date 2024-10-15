using DuitKu.DTOs;
using DuitKu.Services;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DuitKu.Domain;

namespace DuitKu.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController(
        TransactionService transactionService,
        HelperService helperService) : ControllerBase
    {
        private readonly TransactionService _transactionService = transactionService;
        private readonly HelperService _helperService = helperService;

        public Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllTransactionsWith(
            [FromQuery] FilterTransactionDto filterDto)
        {
            Guid userId = GetUserId();
            var totalTransactionsRecord = await _transactionService.GetTotalRecord(userId);

            var transactions = await _transactionService.GetAllTransactionsWith(
                userId,
                filterDto
            );

            var filterResponseApi = _helperService.FilterResponseApi(filterDto.pageNumber ?? 1, filterDto.limit ?? 10, totalTransactionsRecord);

            return Ok(new
            {
                filterResponseApi.isPreviousExists,
                filterResponseApi.isNextExists,
                transactions
            });
        }

        [HttpGet("total-expense")]
        public async Task<ActionResult> GetTotalExpense(
            [FromQuery] TransactionExpenseFilterDto expenseFilterDto)
        {
            Guid userId = GetUserId();
            decimal totalExpense = await _transactionService.GetTotalExpense(userId, expenseFilterDto);

            return Ok(new
            {
                message = "Total pengeluaran kamu",
                totalExpense
            });
        }

        [HttpGet("top-three-expensive")]
        public async Task<ActionResult> GetExpensiveTransaction(
            [FromQuery] TransactionExpenseFilterDto expenseFilterDto
        )
        {
            var transactions = await _transactionService.GetExpensiveTransaction(GetUserId(), expenseFilterDto);

            return Ok(new
            {
                message = "Top 3 Transaksi yang bikin boros numeros wan...",
                transactions
            });
        }

        [HttpGet("{transactionId:guid}")]
        public async Task<ActionResult<Transaction>> Show(Guid transactionId)
        {
            Guid userId = GetUserId();
            var transaction = await _transactionService.GetById(transactionId, userId);

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Store(TransactionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dto.UserId = GetUserId();

            var transaction = await _transactionService.CreateTransaction(dto);

            return CreatedAtAction(nameof(Show), new { transactionId = transaction.Id }, new { transaction, Message = "Ok Transaksi nya udah jadi" });
        }

        [HttpPut("{transactionId:guid}")]
        public async Task<IActionResult> Update(TransactionDto dto, Guid transactionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid userId = GetUserId();
            var transaction = await _transactionService.GetById(transactionId, userId);

            if (transaction == null)
            {
                return BadRequest("Waduh gak bisa update transaksi nya");
            }

            var oldAmount = transaction.Amount;

            transaction.Amount = dto.Amount;
            transaction.AccountId = dto.AccountId;
            transaction.CategoryId = dto.CategoryId;
            transaction.Description = dto.Description;
            transaction.Date = dto.Date ?? DateTime.UtcNow;
            transaction.UserId = userId;

            await _transactionService.UpdateTransaction(transaction, oldAmount);

            return Ok(new { Message = "Ok Transaksi nya berhasil di update" });
        }


        [HttpDelete("{transactionId:guid}")]
        public async Task<IActionResult> Delete(Guid transactionId)
        {
            await _transactionService.DeleteTransaction(transactionId, GetUserId());

            return Ok(new { Message = "Ok Transaksi nya berhasil di hapus" });
        }
    }
}