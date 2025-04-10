using CartingService.Entities;

namespace CartingService.DAL.Interfaces
{
    public interface ICartDAO
    {
        Task<Cart?> GetCartAsync(string key);
        Task AddCartAsync(Cart cart);
        Task DeleteCartAsync(string key);
        Task AddItemToCartAsync(string key, Item item);
        Task DeleteCartItemAsync(string key, int itemId);
        Task UpdateCartAsync(Cart cart);
        Task<IEnumerable<Cart>> GetAllCartsContainingProductAsync(int productId);
    }
}
