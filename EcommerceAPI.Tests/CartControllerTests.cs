using EcommerceAPI.Controllers;
using EcommerceAPI.Models;
using EcommerceAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class CartControllerTests
{
    [Fact]
    public async Task GetCarts_ReturnsOkResult()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.GetCarts()).ReturnsAsync(new List<Cart>());

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.GetCarts();

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetCarts_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.GetCarts()).ReturnsAsync(new List<Cart>());

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.GetCarts();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCart_ReturnsOkResult()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.GetCartById(It.IsAny<int>())).ReturnsAsync(new Cart());

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.GetCart(1);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetCart_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        var mockService = new Mock<ICartService>();
        mockService.Setup(service => service.GetCartById(It.IsAny<int>())).ReturnsAsync(null);

        var controller = new CartController(mockService.Object);

        // Act
        var result = await controller.GetCart(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
