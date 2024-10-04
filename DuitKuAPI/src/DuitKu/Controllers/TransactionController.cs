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
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            return await _transactionService.GetAllTransactions(Guid.Parse(userId));
        }

        [HttpGet("{transactionId:guid}")]
        public async Task<ActionResult<Transaction>> Show(Guid transactionId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var transaction = await _transactionService.GetById(transactionId, Guid.Parse(userId));

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> Store(TransactionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            dto.UserId = Guid.Parse(userId);

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

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var transaction = await _transactionService.GetById(transactionId, Guid.Parse(userId));

            if (transaction == null)
            {
                return BadRequest("Waduh gak bisa update transaksi nya");
            }

            transaction.Amount = dto.Amount;
            transaction.AccountId = dto.AccountId;
            transaction.CategoryId = dto.CategoryId;
            transaction.Description = dto.Description;
            transaction.UserId = Guid.Parse(userId);

            await _transactionService.UpdateTransaction(transaction);

            return Ok(new { Message = "Ok Transaksi nya berhasil di update" });
        }


        [HttpDelete("{transactionId:guid}")]
        public async Task<IActionResult> Delete(Guid transactionId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            await _transactionService.DeleteTransaction(transactionId, Guid.Parse(userId));

            return Ok(new { Message = "Ok Transaksi nya berhasil di hapus" });
        }
    }
}