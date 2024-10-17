using DuitKu.DTOs;
using DuitKu.Services;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DuitKu.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts")]
    public class AccountController(
        AccountService accountService,
        HelperService helperService) : ControllerBase
    {
        private readonly AccountService _accountService = accountService;
        private readonly HelperService _helperService = helperService;

        public Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAccount(
            [FromQuery] BaseParamFilterDto filterDto)
        {
            Guid userId = GetUserId();
            var totalAccountsRecord = await _accountService.GetTotalRecord(userId);

            var accounts = await _accountService.GetAllAccount(userId, filterDto);

            var filterResponseApi = _helperService.FilterResponseApi(filterDto.pageNumber ?? 1, filterDto.limit, totalAccountsRecord);

            return Ok(new
            {
                filterResponseApi.isNextExists,
                filterResponseApi.isPreviousExists,
                accounts
            });
        }

        [HttpGet("total-asset")]
        public async Task<ActionResult> GetTotalAssetFromAccount(
            [FromQuery] AccountTotalIncomeFilterDto totalIncomeFilterDto)
        {
            TotalIncomeDto totalIncome = await _accountService.GetTotalAssetFromAccount(GetUserId(), totalIncomeFilterDto);

            return Ok(new
            {
                message = "Total aset kamu",
                totalIncome
            });
        }

        [HttpGet("transactions")]
        public async Task<IEnumerable<AccountWithTransactionsDto>> GetAllAccountWithTransactions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            return await _accountService.GetAllAccountWithTransactions(Guid.Parse(userId));
        }

        [HttpGet("{accountId:guid}")]
        public async Task<ActionResult<AccountWithTransactionsDto>> Show(Guid accountId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var account = await _accountService.GetById(accountId, Guid.Parse(userId));

            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult> Store(AccountDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            dto.UserId = Guid.Parse(userId);

            var account = await _accountService.CreateAccount(dto);

            return CreatedAtAction(nameof(Show), new { accountId = account.Id }, new { account, Message = "Ok akun nya udah jadi" });
        }

        [HttpPut("{accountId:guid}")]
        public async Task<IActionResult> Update(AccountDto dto, Guid accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var account = await _accountService.GetById(accountId, Guid.Parse(userId));

            if (account == null)
            {
                return BadRequest("Waduh gak bisa update akun nya");
            }

            account.Name = dto.Name;
            account.Balance = dto.Balance;
            account.UserId = Guid.Parse(userId);

            await _accountService.UpdateAccount(account);

            return Ok(new { Message = "Ok akun nya berhasil di update" });
        }

        [HttpDelete("{accountId:guid}")]
        public async Task<IActionResult> Delete(Guid accountId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            int affectedRows = await _accountService.DeleteAccount(accountId, Guid.Parse(userId));

            if (affectedRows > 0)
            {
                return Ok(new { Message = "Ok akun nya berhasil di hapus" });
            }

            return BadRequest(new ProblemDetails
            {
                Title = "Gak bisa hapus akun nya"
            });
        }
    }
}