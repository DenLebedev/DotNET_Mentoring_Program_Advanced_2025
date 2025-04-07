using CartingService.DAL.Interfaces;
using CartingService.Entities;
using LiteDB;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

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

        public async Task<Cart?> GetCartAsync(string key)
        {
            var cart = _cartSet.FindOne(c => c.Key == key);
            if (cart == null)
            {
                return null;
            }
            return await Task.FromResult(cart);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await Task.FromResult(_cartSet.Insert(cart));
        }

        public async Task DeleteCartAsync(string key)
        {
            var cart = _cartSet.FindOne(c => c.Key == key);
            if (cart == null)
            {
                throw new Exception("Cart not found");
            }
            await Task.FromResult(_cartSet.Delete(cart.Key));
        }

        public async Task AddItemToCartAsync(string key, Item item)
        {
            var cart = _cartSet.FindOne(c => c.Key == key);
            if (cart == null)
            {
                throw new Exception("Cart not found");
            }
            cart.Items.Add(item);
            await Task.FromResult(_cartSet.Update(cart));
        }

        public async Task DeleteCartItemAsync(string key, int itemId)
        {
            var cart = _cartSet.FindOne(c => c.Key == key);
            if (cart == null)
            {
                throw new Exception("Cart not found");
            }
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                throw new Exception("Item not found in the cart");
            }
            cart.Items.Remove(item);
            await Task.FromResult(_cartSet.Update(cart));
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            await Task.FromResult(_cartSet.Update(cart));
        }
    }
}
