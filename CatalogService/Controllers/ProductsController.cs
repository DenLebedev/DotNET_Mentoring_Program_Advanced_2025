using AutoMapper;
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
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly IMemoryCache _cache;

        public ProductsController(ProductService productService, IMapper mapper, IUrlHelper urlHelper, IMemoryCache cache)
        {
            _productService = productService;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _cache = cache;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] int? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var cacheKey = $"products_{categoryId?.ToString() ?? "all"}_{page}_{pageSize}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ProductDto> productDtos))
            {
                var products = await _productService.GetProductsAsync(categoryId, page, pageSize);
                productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

                foreach (var productDto in productDtos)
                {
                    CreateLinksForProduct(productDto);
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                _cache.Set(cacheKey, productDtos, cacheOptions);
            }

            return Ok(productDtos);
        }

        [HttpGet("{id}", Name = "GetProductById")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            var productDto = _mapper.Map<ProductDto>(product);
            CreateLinksForProduct(productDto);

            return Ok(productDto);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Post([FromBody] CreateProductDto createDto)
        {
            var createdProduct = await _productService.AddAsync(createDto);
            var result = _mapper.Map<ProductDto>(createdProduct);
            CreateLinksForProduct(result);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProductDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest();

            var updatedProduct = await _productService.UpdateAsync(id, updateDto);
            if (updatedProduct == null)
                return NotFound();

            var result = _mapper.Map<ProductDto>(updatedProduct);
            CreateLinksForProduct(result);
            return Ok(result);
        }

        [HttpDelete("{id}", Name = "DeleteProduct")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/properties")]
        [AllowAnonymous]
        public ActionResult<Dictionary<string, string>> GetProductProperties(int id)
        {
            _ = id;

            // You can extend this to look up real properties per ID if needed.
            var properties = new Dictionary<string, string>
            {
                { "brand", "Samsung" },
                { "model", "S10" },
                { "memory", "128GB" },
                { "color", "Black" }
            };

            return Ok(properties);
        }

        private void CreateLinksForProduct(ProductDto productDto)
        {
            productDto.Links.Add(new Link(_urlHelper.Link("GetProductById", new { id = productDto.Id }), "self", "GET"));
            productDto.Links.Add(new Link(_urlHelper.Link("UpdateProduct", new { id = productDto.Id }), "update_product", "PUT"));
            productDto.Links.Add(new Link(_urlHelper.Link("DeleteProduct", new { id = productDto.Id }), "delete_product", "DELETE"));
        }
    }
}
