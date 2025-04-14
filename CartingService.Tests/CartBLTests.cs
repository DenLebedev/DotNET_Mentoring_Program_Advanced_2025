using AutoMapper;
using CartingService.BLL;
using CartingService.BLL.Interfaces;
using CartingService.DAL.Interfaces;
using CartingService.DTOs;
using CartingService.Entities;
using Moq;
using Xunit;

namespace CartingService.Tests_
{
    public class CartBLTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ICartBL _cartBL;

        public CartBLTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _cartBL = new CartBL(_mockUow.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnCartDto_WhenCartExists()
        {
            // Arrange
            var key = "testKey";
            var cart = new Cart { Key = key, Items = new List<Item>() };
            var cartDto = new CartDto { Key = key, Items = new List<ItemDto>() };

            _mockUow.Setup(u => u.Cart.GetCartAsync(key)).ReturnsAsync(cart);
            _mockMapper.Setup(m => m.Map<CartDto?>(cart)).Returns(cartDto);

            // Act
            var result = await _cartBL.GetCartAsync(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(key, result.Key);
        }

        [Fact]
        public async Task AddCartAsync_ShouldThrowException_WhenCartAlreadyExists()
        {
            // Arrange
            var cartDto = new CartDto { Key = "testKey", Items = new List<ItemDto>() };
            var cart = new Cart { Key = "testKey", Items = new List<Item>() };

            _mockUow.Setup(u => u.Cart.GetCartAsync(cartDto.Key)).ReturnsAsync(cart);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _cartBL.AddCartAsync(cartDto));
        }

        [Fact]
        public async Task AddCartAsync_ShouldAddCart_WhenCartDoesNotExist()
        {
            // Arrange
            var cartDto = new CartDto { Key = "testKey", Items = new List<ItemDto>() };
            var cart = new Cart { Key = "testKey", Items = new List<Item>() };

            _mockUow.Setup(u => u.Cart.GetCartAsync(cartDto.Key)).ReturnsAsync((Cart?)null);
            _mockMapper.Setup(m => m.Map<Cart>(cartDto)).Returns(cart);

            // Act
            await _cartBL.AddCartAsync(cartDto);

            // Assert
            _mockUow.Verify(u => u.Cart.AddCartAsync(cart), Times.Once);
        }

        [Fact]
        public async Task DeleteCartAsync_ShouldThrowException_WhenCartDoesNotExist()
        {
            // Arrange
            var key = "testKey";

            _mockUow.Setup(u => u.Cart.GetCartAsync(key)).ReturnsAsync((Cart?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _cartBL.DeleteCartAsync(key));
        }

        [Fact]
        public async Task DeleteCartAsync_ShouldDeleteCart_WhenCartExists()
        {
            // Arrange
            var key = "testKey";
            var cart = new Cart { Key = key, Items = new List<Item>() };

            _mockUow.Setup(u => u.Cart.GetCartAsync(key)).ReturnsAsync(cart);

            // Act
            await _cartBL.DeleteCartAsync(key);

            // Assert
            _mockUow.Verify(u => u.Cart.DeleteCartAsync(key), Times.Once);
        }

        [Fact]
        public async Task AddItemToCartAsync_ShouldCreateCart_WhenCartDoesNotExist()
        {
            // Arrange
            var key = "testKey";
            var itemDto = new ItemDto { Id = 1, Name = "Item1", Quantity = 1 };
            var item = new Item { Id = 1, Name = "Item1", Quantity = 1, ImageURL = "http://example.com/image.jpg", ImageAltText = "Item Image" };

            _mockUow.Setup(u => u.Cart.GetCartAsync(key)).ReturnsAsync((Cart?)null);
            _mockMapper.Setup(m => m.Map<Item>(itemDto)).Returns(item);

            // Act
            await _cartBL.AddItemToCartAsync(key, itemDto);

            // Assert
            _mockUow.Verify(u => u.Cart.AddCartAsync(It.Is<Cart>(c => c.Items.Contains(item))), Times.Once);
        }
    }
}
