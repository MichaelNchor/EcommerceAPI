using EcommerceAPI.Controllers;
using EcommerceAPI.Data;
using EcommerceAPI.DTO.Product;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EcommerceAPI.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task Get_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(service => service.GetProducts()).ReturnsAsync(new List<ProductGetDTO>());

            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenProductNotFound()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(service => service.GetProduct(It.IsAny<int>())).ReturnsAsync((ProductGetDTO)null);

            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Put_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<IProductService>();
            mockService.Setup(service => service.UpdateProduct(It.IsAny<int>(), It.IsAny<ProductPutDTO>())).ReturnsAsync(new ProductPutDTO());

            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Put(1, new ProductPutDTO { ProductId = 1 });

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var mockService = new Mock<IProductService>();

            var controller = new ProductController(mockService.Object);

            // Act
            var result = await controller.Put(1, new ProductPutDTO { ProductId = 2 });

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

    }
}
