using AutoMapper;
using CatalogService.Application.Common;
using CatalogService.Application.DTOs;
using CatalogService.Application.Services;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;

        public CategoriesController(CategoryService categoryService, IMapper mapper, IUrlHelper urlHelper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            foreach (var categoryDto in categoryDtos)
            {
                CreateLinksForCategory(categoryDto);
            }

            return Ok(categoryDtos);
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            CreateLinksForCategory(categoryDto);

            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryDto createDto)
        {
            var createdCategory = await _categoryService.AddAsync(createDto);
            var result = _mapper.Map<CategoryDto>(createdCategory);
            CreateLinksForCategory(result);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}", Name = "UpdateCategory")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateCategoryDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest();

            var updatedCategory = await _categoryService.UpdateAsync(id, updateDto);
            if (updatedCategory == null)
                return NotFound();

            var result = _mapper.Map<CategoryDto>(updatedCategory);
            CreateLinksForCategory(result);
            return Ok(result);
        }

        [HttpDelete("{id}", Name = "DeleteCategory")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}/with-products", Name = "DeleteCategoryWithProducts")]
        public async Task<IActionResult> DeleteCateoryWithProducts(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryWithProductsAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        private void CreateLinksForCategory(CategoryDto categoryDto)
        {
            categoryDto.Links.Add(new Link(_urlHelper.Link("GetCategoryById", new { id = categoryDto.Id }), "self", "GET"));
            categoryDto.Links.Add(new Link(_urlHelper.Link("UpdateCategory", new { id = categoryDto.Id }), "update_category", "PUT"));
            categoryDto.Links.Add(new Link(_urlHelper.Link("DeleteCategory", new { id = categoryDto.Id }), "delete_category", "DELETE"));
        }
    }
}