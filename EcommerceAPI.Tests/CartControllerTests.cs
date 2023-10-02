using EcommerceAPI.Controllers;
using EcommerceAPI.Models;
using EcommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

public class CartControllerTests
{
    [Fact]
    public async Task GetCarts_ReturnsOk_WhenServiceReturnsData()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        var cartItems = new List<Cart>
        {
            new Cart { CartId = 1, Quantity = 2 },
            new Cart { CartId = 2, Quantity = 3 },
        };
        mockService.Setup(service => service.GetCarts()).ReturnsAsync(cartItems);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.GetCarts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var items = Assert.IsAssignableFrom<IEnumerable<Cart>>(okResult.Value);
        Assert.Equal(2, items.Count());
    }

    [Fact]
    public async Task GetCarts_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.GetCarts()).ReturnsAsync((IEnumerable<Cart>)null);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.GetCarts();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCartProductsAsync_ReturnsOk_WhenServiceReturnsData()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        var cartItems = new List<Cart>
        {
            new Cart { CartId = 1, Quantity = 2 },
            new Cart { CartId = 2, Quantity = 3 },
        };
        mockService.Setup(service => service.GetCartsQueryable(null, null, null)).ReturnsAsync(cartItems);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.GetCartProductsAsync(null, null, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<Cart>>(okResult.Value);
        Assert.Equal(2, items.Count());
    }

    [Fact]
    public async Task GetCartProductsAsync_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.GetCartsQueryable(null, null, null)).ReturnsAsync((IEnumerable<Cart>)null);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.GetCartProductsAsync(null, null, null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetCart_ReturnsOk_WhenServiceReturnsData()
    {
        // Arrange
        var cartId = 1;
        var mockService = new Mock<ICartService>();
        var cartItem = new Cart { CartId = cartId, Quantity = 2 };
        mockService.Setup(service => service.GetCartById(cartId)).ReturnsAsync(cartItem);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.GetCart(cartId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var item = Assert.IsType<Cart>(okResult.Value);
        Assert.Equal(cartId, item.CartId);
    }

    [Fact]
    public async Task GetCart_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        var cartId = 1;
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.GetCartById(cartId)).ReturnsAsync((Cart)null);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.GetCart(cartId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddToCart_ReturnsCreatedAtAction_WhenProductExists()
    {
        // Arrange
        var productId = 1;
        var quantity = 2;
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.ProductExists(productId)).Returns(true);
        mockService.Setup(service => service.CartExistsWithProduct(productId)).Returns(false);
        var cartItem = new Cart { CartId = 1, Quantity = quantity };
        mockService.Setup(service => service.AddToNewCart(productId, quantity)).ReturnsAsync(cartItem);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.AddToCart(productId, quantity);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var item = Assert.IsType<Cart>(createdAtActionResult.Value);
        Assert.Equal(productId, item.CartId);
    }

    [Fact]
    public async Task AddToCart_ReturnsCreatedAtAction_WhenProductExistsInCart()
    {
        // Arrange
        var productId = 1;
        var quantity = 2;
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.ProductExists(productId)).Returns(true);
        mockService.Setup(service => service.CartExistsWithProduct(productId)).Returns(true);
        var cartItem = new Cart { CartId = 1, Quantity = quantity };
        mockService.Setup(service => service.UpdateExistingCart(productId, quantity)).ReturnsAsync(cartItem);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.AddToCart(productId, quantity);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var item = Assert.IsType<Cart>(createdAtActionResult.Value);
        Assert.Equal(productId, item.CartId);
    }

    [Fact]
    public async Task AddToCart_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 1;
        var quantity = 2;
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.ProductExists(productId)).Returns(false);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.AddToCart(productId, quantity);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteCart_ReturnsOk_WhenServiceReturnsData()
    {
        // Arrange
        var cartId = 1;
        var mockService = new Mock<ICartService>();
        var cartItem = new Cart { CartId = cartId, Quantity = 2 };
        mockService.Setup(service => service.DeleteCart(cartId)).ReturnsAsync(cartItem);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.DeleteCart(cartId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var item = Assert.IsType<Cart>(okResult.Value);
        Assert.Equal(cartId, item.CartId);
    }

    [Fact]
    public async Task DeleteCart_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        var cartId = 1;
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.DeleteCart(cartId)).ReturnsAsync((Cart)null);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.DeleteCart(cartId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
