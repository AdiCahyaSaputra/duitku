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
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            return await _categoryService.GetAllCategories(Guid.Parse(userId));
        }

        [HttpGet("{categoryId:guid}")]
        public async Task<ActionResult<Category>> Show(Guid categoryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var category = await _categoryService.GetById(categoryId, Guid.Parse(userId));

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Store(CategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            dto.UserId = Guid.Parse(userId);

            var category = await _categoryService.CreateCategory(dto);

            return CreatedAtAction(nameof(Show), new { categoryId = category.Id }, new { category, Message = "Ok kategori nya udah jadi" });
        }

        [HttpPut("{categoryId:guid}")]
        public async Task<IActionResult> Update(CategoryDto dto, Guid categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var category = await _categoryService.GetById(categoryId, Guid.Parse(userId));

            if (category == null)
            {
                return BadRequest("Waduh gak bisa update kategori nya");
            }

            category.Name = dto.Name;
            category.UserId = Guid.Parse(userId);

            await _categoryService.UpdateCategory(category);

            return Ok(new { Message = "Ok kategori nya berhasil di update" });
        }


        [HttpDelete("{categoryId:guid}")]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            await _categoryService.DeleteCategory(categoryId, Guid.Parse(userId));

            return Ok(new { Message = "Ok kategori nya berhasil di hapus" });
        }
    }
}