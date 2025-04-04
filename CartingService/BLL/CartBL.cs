using CartingService.BLL.Interfaces;
using CartingService.DAL.Interfaces;
using CartingService.Entities;

namespace CartingService.BLL
{
    public class CartBL : ICartBL
    {
        private readonly IUnitOfWork _uow;

        #region Constructor
        public CartBL(IUnitOfWork uow)
        {
            _uow = uow;
        }

        #endregion

        // TODO: Implement the validation in methods in the BLL layer
        // Use FluentValidation or Data Annotations for validation

        public Cart GetCartById(int id)
        {
            return _uow.Cart.GetCartById(id);
        }

        public void AddCart(Cart cart)
        {
            _uow.Cart.AddCart(cart);
        }

        public void DeleteCart(int id)
        {
            _uow.Cart.DeleteCart(id);
        }

        public IEnumerable<Item> GetItems(int cartId)
        {
            return _uow.Cart.GetItemsByCartId(cartId);
        }

        public void AddItem(int cartId, Item item)
        {
            _uow.Cart.AddItemToCart(cartId, item);
        }

        public void RemoveItem(int cartId, int itemId)
        {
            _uow.Cart.RemoveItemFromCart(cartId, itemId);
        }
    }
}
