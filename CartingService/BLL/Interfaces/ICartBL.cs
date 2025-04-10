using CartingService.DTOs;
using CartingService.Entities;

namespace CartingService.BLL.Interfaces
{
    public interface ICartBL
    {
        Task<CartDto?> GetCartAsync(string key);
        Task AddCartAsync(CartDto cartDto);
        Task DeleteCartAsync(string key);
        Task AddItemToCartAsync(string key, ItemDto itemDto);
        Task DeleteItemFromCartAsync(string key, int itemId);
        Task UpdateItemInAllCartsAsync(int productId, string newName, decimal newPrice);
    }
}
