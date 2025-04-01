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

        public CartingServiceController(ILogger<CartingServiceController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetCart")]
        public Cart GetCartById(int id)
        {
            return new Cart
            {
                Id = id,
                Items = new List<Item>()
            };
        }
    }
}
