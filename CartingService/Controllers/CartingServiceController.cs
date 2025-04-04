using CartingService.BLL;
using CartingService.BLL.Interfaces;
using CartingService.DAL.Interfaces;
using CartingService.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartingServiceController : ControllerBase
    {
        private readonly ILogger<CartingServiceController> _logger;
        private readonly ICartBL _cartBL;

        public CartingServiceController(ILogger<CartingServiceController> logger, ICartBL cartBL)
        {
            _logger = logger;
            _cartBL = cartBL;
        }

        [HttpGet("{id}", Name = "GetCart")]
        public IActionResult GetCart(int id)
        {
            var cart = _cartBL.GetCartById(id);
            return Ok(cart);
        }

        [HttpGet("items/{cartId}", Name = "GetItemsByCartId")]
        public IActionResult GetItemsByCartId(int cartId)
        {
            var items = _cartBL.GetItems(cartId);
            return Ok(items);
        }

        [HttpPost(Name = "AddCart")]
        public IActionResult AddCart(Cart cart)
        {
            _cartBL.AddCart(cart);
            return Ok();
        }

        [HttpPut("add-item/{cartId}", Name = "AddItemToCart")]
        public IActionResult AddItemToCart(int cartId, Item item)
        {
            _cartBL.AddItem(cartId, item);
            return Ok();
        }

        [HttpPut("remove-item/{cartId}/{itemId}", Name = "RemoveItemFromCart")]
        public IActionResult RemoveItemFromCart(int cartId, int itemId)
        {
            _cartBL.RemoveItem(cartId, itemId);
            return Ok();
        }

        [HttpDelete(Name = "DeleteCart")]
        public IActionResult DeleteCart(int id)
        {
            _cartBL.DeleteCart(id);
            return Ok();
        }
    }
}
