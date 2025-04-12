using Microsoft.AspNetCore.Mvc;
using CartingService.BLL.Interfaces;
using CartingService.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace CartingService.Controllers.V2
{
    [Authorize(Roles = "Manager,StoreCustomer")]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("api/v{version:apiVersion}/cart")]
    [ApiController]
    public class CartV2Controller : ControllerBase
    {
        private readonly ILogger<CartV2Controller> _logger;
        private readonly ICartBL _cartBL;

        public CartV2Controller(ILogger<CartV2Controller> logger, ICartBL cartBL)
        {
            _logger = logger;
            _cartBL = cartBL;
        }

        [HttpGet("{key}/items")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetCartItems(string key)
        {
            var cart = await _cartBL.GetCartAsync(key);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart.Items);
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