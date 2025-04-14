using AutoMapper;
using CartingService.BLL.Interfaces;
using CartingService.DAL.Interfaces;
using CartingService.DTOs;
using CartingService.Entities;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace CartingService.BLL
{
    public class CartBL : ICartBL
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CartBL(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CartDto?> GetCartAsync(string key)
        {
            var cart = await _uow.Cart.GetCartAsync(key);
            return _mapper.Map<CartDto?>(cart);
        }

        public async Task AddCartAsync(CartDto cartDto)
        {
            var existingCart = await _uow.Cart.GetCartAsync(cartDto.Key);
            if (existingCart != null)
            {
                throw new InvalidOperationException("A cart with this key already exists.");
            }

            var cart = _mapper.Map<Cart>(cartDto);

            await _uow.Cart.AddCartAsync(cart);
        }

        public async Task DeleteCartAsync(string key)
        {
            var cart = await _uow.Cart.GetCartAsync(key);
            if (cart == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }
            await _uow.Cart.DeleteCartAsync(key);
        }

        public async Task AddItemToCartAsync(string key, ItemDto itemDto)
        {
            var cart = await _uow.Cart.GetCartAsync(key) ?? new Cart { Key = key, Items = new List<Item>() };
            var item = _mapper.Map<Item>(itemDto);
            cart.Items.Add(item);
            if (cart.Items.Count == 1)
            {
                await _uow.Cart.AddCartAsync(cart);
            }
            else
            {
                await _uow.Cart.UpdateCartAsync(cart);
            }
        }

        public async Task DeleteItemFromCartAsync(string key, int itemId)
        {
            await _uow.Cart.DeleteCartItemAsync(key, itemId);
        }

        public async Task UpdateItemInAllCartsAsync(int productId, string newName, decimal newPrice)
        {
            var carts = await _uow.Cart.GetAllCartsContainingProductAsync(productId);

            foreach (var cart in carts)
            {
                foreach (var item in cart.Items)
                {
                    if (item.Id == productId)
                    {
                        item.Name = newName;
                        item.Price = newPrice;
                    }
                }

                await _uow.Cart.UpdateCartAsync(cart);
            }
        }
    }
}
