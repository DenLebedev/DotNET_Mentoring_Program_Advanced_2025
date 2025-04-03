using CartingService.DAL.Interfaces;
using CartingService.Entities;
using LiteDB;

namespace CartingService.DAL
{
    public class CartDAO : ICartDAO
    {
        private LiteDatabase database;
        private ILiteCollection<Cart> cartSet;

        public CartDAO(LiteDatabase context)
        {
            database = context;
            cartSet = database.GetCollection<Cart>("carts");
        }

        public Cart GetCartById(int id)
        {
            var cart = cartSet.Query()
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
            cartSet.Insert(cart);
        }

        public void DeleteCart(int id)
        {
            cartSet.Delete(id);
        }

        public void AddItemToCart(int cartId, Item item)
        {
            var cart = cartSet.FindById(cartId);
            if (cart != null)
            {
                cart.Items.Add(item);
                cartSet.Update(cart);
            }
            else
            {
                // Handle the case where the cart does not exist
                throw new Exception("Cart not found");
            }
        }

        public void RemoveItemFromCart(int cartId, int itemId)
        {
            var cart = cartSet.FindById(cartId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    cartSet.Update(cart);
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
            var cart = cartSet.FindById(cartId);
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
