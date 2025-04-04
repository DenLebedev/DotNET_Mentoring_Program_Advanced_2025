using CartingService.BLL;
using CartingService.DAL.Interfaces;
using CartingService.Entities;
using Moq;
using Xunit;

namespace CartingService.Tests_
{
    public class CartBLTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICartDAO> _mockCartDAO;
        private readonly CartBL _cartBL;

        public CartBLTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCartDAO = new Mock<ICartDAO>();
            _mockUnitOfWork.Setup(uow => uow.Cart).Returns(_mockCartDAO.Object);
            _cartBL = new CartBL(_mockUnitOfWork.Object);
        }

        [Fact]
        public void GetCartById_ShouldReturnCart_WhenCartExists()
        {
            // Arrange
            var cartId = 1;
            var expectedCart = new Cart { Id = cartId, Items = new List<Item>() };
            _mockCartDAO.Setup(dao => dao.GetCartById(cartId)).Returns(expectedCart);

            // Act
            var result = _cartBL.GetCartById(cartId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cartId, result.Id);
        }

        [Fact]
        public void GetCartById_ShouldReturnNull_WhenCartDoesNotExist()
        {
            // Arrange
            var cartId = 1;
            _mockCartDAO.Setup(dao => dao.GetCartById(cartId)).Returns((Cart)null);

            // Act
            var result = _cartBL.GetCartById(cartId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void AddCart_ShouldCallAddCartOnUow()
        {
            // Arrange
            var cart = new Cart { Id = 1, Items = new List<Item>() };

            // Act
            _cartBL.AddCart(cart);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.Cart.AddCart(cart), Times.Once);
        }

        [Fact]
        public void RemoveItem_ShouldCallRemoveItemFromCartOnUow()
        {
            // Arrange
            var cartId = 1;
            var itemId = 1;

            // Act
            _cartBL.RemoveItem(cartId, itemId);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.Cart.RemoveItemFromCart(cartId, itemId), Times.Once);
        }

        [Fact]
        public void GetItems_ShouldReturnItems_WhenCartExists()
        {
            // Arrange
            int cartId = 1;
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Item1", ImageURL = "url1", ImageAltText = "alt1", Price = 10.0m, Quantity = 1 },
                new Item { Id = 2, Name = "Item2", ImageURL = "url2", ImageAltText = "alt2", Price = 20.0m, Quantity = 2 }
            };
            _mockCartDAO.Setup(dao => dao.GetItemsByCartId(cartId)).Returns(items);

            // Act
            var result = _cartBL.GetItems(cartId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(items, result);
        }

        [Fact]
        public void GetItems_ShouldReturnEmptyList_WhenCartDoesNotExist()
        {
            // Arrange
            int cartId = 1;
            _mockCartDAO.Setup(dao => dao.GetItemsByCartId(cartId)).Returns(new List<Item>());

            // Act
            var result = _cartBL.GetItems(cartId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void AddItem_ShouldCallAddItemToCart()
        {
            // Arrange
            int cartId = 1;
            var item = new Item { Id = 1, Name = "Test Item", ImageURL = "http://example.com/image.jpg", ImageAltText = "Image", Price = 10.0m, Quantity = 1 };

            // Act
            _cartBL.AddItem(cartId, item);

            // Assert
            _mockCartDAO.Verify(dao => dao.AddItemToCart(cartId, item), Times.Once);
        }

        [Fact]
        public void RemoveItem_ShouldCallRemoveItemFromCart()
        {
            // Arrange
            int cartId = 1;
            int itemId = 1;

            // Act
            _cartBL.RemoveItem(cartId, itemId);

            // Assert
            _mockCartDAO.Verify(cart => cart.RemoveItemFromCart(cartId, itemId), Times.Once);
        }
    }
}