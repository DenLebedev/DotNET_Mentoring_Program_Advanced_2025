using Microsoft.AspNetCore.Mvc;
using CartingService.BLL.Interfaces;
using CartingService.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace CartingService.Controllers.V1
{
    [Authorize(Roles = "Manager,StoreCustomer")]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/cart")]
    [ApiController]
    public class CartV1Controller : ControllerBase
    {
        private readonly ILogger<CartV1Controller> _logger;
        private readonly ICartBL _cartBL;

        public CartV1Controller(ILogger<CartV1Controller> logger, ICartBL cartBL)
        {
            _logger = logger;
            _cartBL = cartBL;
        }

        [HttpGet("{key}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<CartDto>> GetCart(string key)
        {
            var cart = await _cartBL.GetCartAsync(key);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpPost("{key}/items")]
        public async Task<IActionResult> AddItemToCart(string key, [FromBody] ItemDto itemDto)
        {
            await _cartBL.AddItemToCartAsync(key, itemDto);
            return Ok();
        }

        [HttpDelete("{key}/items/{itemId}")]
        public async Task<IActionResult> DeleteItemFromCart(string key, int itemId)
        {
            await _cartBL.DeleteItemFromCartAsync(key, itemId);
            return Ok();
        }
    }
}
