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
    [Route("api/sub-categories")]
    public class SubCategoryController : ControllerBase
    {
        private readonly SubCategoryService _subCategoryService;
        private readonly HelperService _helperService;

        public SubCategoryController(SubCategoryService subCategoryService, HelperService helperService)
        {
            _subCategoryService = subCategoryService;
            _helperService = helperService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllSubCategories(
            [FromQuery] GetAllSubCategoriesFilterDto filterDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var totalSubCategoriesRecord = await _subCategoryService.GetTotalRecord(Guid.Parse(userId));

            var subCategories = await _subCategoryService.GetAllSubCategories(Guid.Parse(userId), filterDto);

            var filterResponseApi = _helperService.FilterResponseApi(filterDto.pageNumber ?? 1, filterDto.limit, totalSubCategoriesRecord);

            return Ok(new
            {
                filterResponseApi.isNextExists,   
                filterResponseApi.isPreviousExists,   
                subCategories
            });
        }

        [HttpGet("{subCategoryId:guid}")]
        public async Task<ActionResult<SubCategory>> Show(Guid subCategoryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var subCategory = await _subCategoryService.GetById(subCategoryId, Guid.Parse(userId));

            return Ok(subCategory);
        }

        [HttpPost]
        public async Task<ActionResult> Store(SubCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            dto.UserId = Guid.Parse(userId);

            var subCategory = await _subCategoryService.CreateSubCategory(dto);

            return CreatedAtAction(nameof(Show), new { subCategoryId = subCategory.Id }, new { subCategory, Message = "Ok sub kategori nya udah jadi" });
        }

        [HttpPut("{subCategoryId:guid}")]
        public async Task<IActionResult> Update(SubCategoryDto dto, Guid subCategoryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var subCategory = await _subCategoryService.GetById(subCategoryId, Guid.Parse(userId));

            if (subCategory == null)
            {
                return BadRequest("Waduh gak bisa update sub kategori nya");
            }

            subCategory.Name = dto.Name;
            subCategory.CategoryId = dto.CategoryId == Guid.Empty ? subCategory.CategoryId : dto.CategoryId;
            subCategory.UserId = Guid.Parse(userId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _subCategoryService.UpdateSubCategory(subCategory);

            return Ok(new { Message = "Ok sub kategori nya berhasil di update" });
        }


        [HttpDelete("{subCategoryId:guid}")]
        public async Task<IActionResult> Delete(Guid subCategoryId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            await _subCategoryService.DeleteSubCategory(subCategoryId, Guid.Parse(userId));

            return Ok(new { Message = "Ok sub kategori nya berhasil di hapus" });
        }
    }
}