﻿using AutoMapper;
using CatalogService.Application.Common;
using CatalogService.Application.DTOs;
using CatalogService.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(CategoryService categoryService, IMapper mapper, IUrlHelper urlHelper, IMemoryCache cache, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var correlationId = GetCorrelationId();
            // _logger.LogInformation("Received GET /api/categories with Correlation ID: {CorrelationId}", correlationId);

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
            {
                _logger.LogInformation("Received GET /api/categories");

                const string cacheKey = "category_list";

                if (!_cache.TryGetValue(cacheKey, out IEnumerable<CategoryDto> categoryDtos))
                {
                    var categories = await _categoryService.GetAllAsync();
                    categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                    foreach (var categoryDto in categoryDtos)
                    {
                        CreateLinksForCategory(categoryDto);
                    }

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                    _cache.Set(cacheKey, categoryDtos, cacheEntryOptions);
                }

                return Ok(categoryDtos);
            }
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        [AllowAnonymous]
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
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Post([FromBody] CreateCategoryDto createDto)
        {
            var correlationId = GetCorrelationId();

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
            {
                _logger.LogInformation("Received POST /api/categories");

                var createdCategory = await _categoryService.AddAsync(createDto);
                var result = _mapper.Map<CategoryDto>(createdCategory);
                CreateLinksForCategory(result);

                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
        }

        [HttpPut("{id}", Name = "UpdateCategory")]
        [Authorize(Roles = "Manager")]
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
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}/with-products", Name = "DeleteCategoryWithProducts")]
        [Authorize(Roles = "Manager")]
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


        private string GetCorrelationId()
        {
            return HttpContext.Items.TryGetValue("X-Correlation-ID", out var value) ? value?.ToString() ?? "" : "";
        }


        private void CreateLinksForCategory(CategoryDto categoryDto)
        {
            categoryDto.Links.Add(new Link(_urlHelper.Link("GetCategoryById", new { id = categoryDto.Id }), "self", "GET"));
            categoryDto.Links.Add(new Link(_urlHelper.Link("UpdateCategory", new { id = categoryDto.Id }), "update_category", "PUT"));
            categoryDto.Links.Add(new Link(_urlHelper.Link("DeleteCategory", new { id = categoryDto.Id }), "delete_category", "DELETE"));
        }
    }
}
