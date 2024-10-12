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
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly HelperService _helperService;

        public AccountController(
            AccountService accountService,
            HelperService helperService)
        {
            _accountService = accountService;
            _helperService = helperService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAccount(
            [FromQuery] BaseParamFilterDto filterDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var totalAccountsRecord = await _accountService.GetTotalRecord(Guid.Parse(userId));

            var accounts = await _accountService.GetAllAccount(Guid.Parse(userId), filterDto);

            var filterResponseApi = _helperService.FilterResponseApi(filterDto.pageNumber ?? 1, filterDto.limit ?? 10, totalAccountsRecord);

            return Ok(new
            {
                filterResponseApi.isNextExists,
                filterResponseApi.isPreviousExists,
                accounts
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

            await _accountService.DeleteAccount(accountId, Guid.Parse(userId));

            return Ok(new { Message = "Ok akun nya berhasil di hapus" });
        }
    }
}