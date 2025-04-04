using CartingService.Entities;

namespace CartingService.DAL.Interfaces
{
    public interface ICartDAO
    {
        Cart GetCartById(int id);
        void AddCart(Cart cart);
        void DeleteCart(int id);
        void AddItemToCart(int cartId, Item item);
        void RemoveItemFromCart(int cartId, int itemId);
        List<Item> GetItemsByCartId(int cartId);
    }
}
