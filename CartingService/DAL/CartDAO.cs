using CartingService.DAL.Interfaces;
using CartingService.Entities;
using LiteDB;

namespace CartingService.DAL
{
    public class CartDAO : ICartDAO
    {
        private readonly IUnitOfWork _context;
        private readonly ILiteCollection<Cart> _cartSet;

        public CartDAO(IUnitOfWork context)
        {
            _context = context;
            _cartSet = _context.GetCartCollection();
        }

        public Cart GetCartById(int id)
        {
            var cart = _cartSet.Query()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (cart != null)
            {
                return cart;
            }
            else
            {
                throw new Exception("Cart not found");
            }
        }

        public void AddCart(Cart cart)
        {
            _cartSet.Insert(cart);
        }

        public void DeleteCart(int id)
        {
            _cartSet.Delete(id);
        }

        public void AddItemToCart(int cartId, Item item)
        {
            var cart = _cartSet.FindById(cartId);
            if (cart != null)
            {
                cart.Items.Add(item);
                _cartSet.Update(cart);
            }
            else
            {
                // Handle the case where the cart does not exist
                throw new Exception("Cart not found");
            }
        }

        public void RemoveItemFromCart(int cartId, int itemId)
        {
            var cart = _cartSet.FindById(cartId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    _cartSet.Update(cart);
                }
                else
                {
                    // Handle the case where the item does not exist in the cart
                    throw new Exception("Item not found in the cart");
                }
            }
            else
            {
                // Handle the case where the cart does not exist
                throw new Exception("Cart not found");
            }
        }
        public List<Item> GetItemsByCartId(int cartId)
        {
            var cart = _cartSet.FindById(cartId);
            if (cart != null)
            {
                return cart.Items;
            }
            else
            {
                throw new Exception("Cart not found");
            }
        }
    }
}
