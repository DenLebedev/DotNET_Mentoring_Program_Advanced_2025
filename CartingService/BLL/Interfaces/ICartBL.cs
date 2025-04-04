using CartingService.Entities;

namespace CartingService.BLL.Interfaces
{
    public interface ICartBL
    {
        Cart GetCartById(int id);
        void AddCart(Cart cart);
        void DeleteCart(int id);
        IEnumerable<Item> GetItems(int cartID);
        void AddItem(int cartId, Item item);
        void RemoveItem(int cartId, int itemId);
    }
}
