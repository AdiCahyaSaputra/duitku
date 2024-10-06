using DuitKu.Services;
using DuitKu.Persistance.Database;
using DuitKu.DTOs;
using DuitKu.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DuitKu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ApplicationDBContext _context;

        public AuthController(AuthService authService, ApplicationDBContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            // Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _authService.Register(dto);

                if (string.IsNullOrEmpty(token)) return BadRequest("Waduh Gagal registrasi nih bre");

                return Ok(new { token });
            }
            catch (System.Exception)
            {
                return StatusCode(500, new { Message = "Waduh gak bisa register nih bre" });
            }

        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            // Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = _authService.Login(dto);

            if (token == null) return Unauthorized();

            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("user")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var authUser = await _context.User
                .Where(user => user.Id == Guid.Parse(userId))
                .Select(user => new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                })
                .FirstAsync();

            return Ok(new { user = authUser });
        }
    }
}