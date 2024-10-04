using DuitKu.Services;
using DuitKu.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DuitKu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
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
    }
}