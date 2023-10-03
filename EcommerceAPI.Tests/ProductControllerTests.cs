//using EcommerceAPI.Controllers;
//using EcommerceAPI.DTO.Product;
//using EcommerceAPI.Data;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//public class ProductControllerTests
//{
//    [Fact]
//    //public async Task GetProducts_ReturnsOkResult_WhenServiceReturnsData()
//    //{
//    //    // Arrange
//    //    var mockService = new Mock<IProductService>();
//    //    mockService.Setup(service => service.GetProducts()).ReturnsAsync(new List<ProductGetDTO> { new ProductGetDTO() });

//    //    var controller = new ProductController(mockService.Object);

//    //    // Act
//    //    var result = await controller.Gets();

//    //    // Assert
//    //    var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
//    //    var products = Assert.IsAssignableFrom<IEnumerable<ProductGetDTO>>(okObjectResult.Value);
//    //    Assert.NotEmpty(products);
//    //}

//    [Fact]
//    public async Task GetProducts_ReturnsNotFound_WhenServiceReturnsNull()
//    {
//        // Arrange
//        var mockService = new Mock<IProductService>();
//        mockService.Setup(service => service.GetProducts()).ReturnsAsync((IEnumerable<ProductGetDTO>)null);

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Gets();

//        // Assert
//        Assert.IsType<NotFoundResult>(result.Result);
//    }

//    [Fact]
//    public async Task Get_ReturnsOkResult_WhenServiceReturnsData()
//    {
//        // Arrange
//        var productId = 1;
//        var productGetDTO = new ProductGetDTO { ProductId = productId };
//        var mockService = new Mock<IProductService>();
//        mockService.Setup(service => service.GetProduct(productId)).ReturnsAsync(productGetDTO);

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Get(productId);

//        // Assert
//        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
//        var product = Assert.IsType<ProductGetDTO>(okObjectResult.Value);
//        Assert.Equal(productId, product.ProductId);
//    }

//    [Fact]
//    public async Task Get_ReturnsNotFound_WhenServiceReturnsNull()
//    {
//        // Arrange
//        var productId = 1;
//        var mockService = new Mock<IProductService>();
//        mockService.Setup(service => service.GetProduct(productId)).ReturnsAsync((ProductGetDTO)null);

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Get(productId);

//        // Assert
//        Assert.IsType<NotFoundResult>(result.Result);
//    }

//    [Fact]
//    public async Task Get_ReturnsNotFound_WhenProductDoesNotExist()
//    {
//        // Arrange
//        var productId = 1;
//        var mockService = new Mock<IProductService>();
//        mockService.Setup(service => service.GetProduct(productId)).ReturnsAsync((ProductGetDTO)null);

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Get(productId);

//        // Assert
//        Assert.IsType<NotFoundResult>(result.Result);
//    }

//    [Fact]
//    public async Task Put_ReturnsOkResult_WhenUpdateIsSuccessful()
//    {
//        // Arrange
//        var productId = 1;
//        var productPutDTO = new ProductPutDTO { ProductId = productId };
//        var mockService = new Mock<IProductService>();
//        mockService.Setup(service => service.UpdateProduct(productId, productPutDTO)).ReturnsAsync(productPutDTO);

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Put(productId, productPutDTO);

//        // Assert
//        var okObjectResult = Assert.IsType<OkObjectResult>(result);
//        var product = Assert.IsType<ProductPutDTO>(okObjectResult.Value);
//        Assert.Equal(productId, product.ProductId);
//    }

//    [Fact]
//    public async Task Put_ReturnsBadRequest_WhenIdDoesNotMatchProduct()
//    {
//        // Arrange
//        var productId = 1;
//        var productPutDTO = new ProductPutDTO { ProductId = 2 }; // Mismatched IDs
//        var mockService = new Mock<IProductService>();

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Put(productId, productPutDTO);

//        // Assert
//        Assert.IsType<BadRequestResult>(result);
//    }

//    [Fact]
//    public async Task Put_ReturnsNotFound_WhenServiceReturnsNull()
//    {
//        // Arrange
//        var productId = 1;
//        var productPutDTO = new ProductPutDTO { ProductId = productId };
//        var mockService = new Mock<IProductService>();
//        mockService.Setup(service => service.UpdateProduct(productId, productPutDTO)).ReturnsAsync((ProductPutDTO)null);

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Put(productId, productPutDTO);

//        // Assert
//        Assert.IsType<NotFoundResult>(result);
//    }

//    [Fact]
//    public async Task Post_ReturnsCreatedAtAction_WhenProductIsAddedSuccessfully()
//    {
//        // Arrange
//        var productAddDTO = new ProductAddDTO { ProductId = 1 };
//        var mockService = new Mock<IProductService>();
//        mockService.Setup(service => service.AddProduct(productAddDTO)).ReturnsAsync(productAddDTO);

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Post(productAddDTO);

//        // Assert
//        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
//        Assert.Equal("GetProduct", createdAtActionResult.ActionName);
//        Assert.Equal(productAddDTO.ProductId, createdAtActionResult.RouteValues["id"]);
//    }

//    [Fact]
//    public async Task Post_ReturnsBadRequest_WhenModelStateIsInvalid()
//    {
//        // Arrange
//        var productAddDTO = new ProductAddDTO { ProductId = 1 };
//        var mockService = new Mock<IProductService>();

//        var controller = new ProductController(mockService.Object);
//        controller.ModelState.AddModelError("PropertyName", "Error Message"); // Simulating invalid model state

//        // Act
//        var result = await controller.Post(productAddDTO);

//        // Assert
//        Assert.IsType<BadRequestResult>(result.Result);
//    }

//    [Fact]
//    public async Task Delete_ReturnsOkResult_WhenProductIsDeletedSuccessfully()
//    {
//        // Arrange
//        var productId = 1;
//        var productDeleteDTO = new ProductDeleteDTO { ProductId = productId };
//        var mockService = new Mock<IProductService>();
//        mockService.Setup(service => service.DeleteProduct(productId)).ReturnsAsync(productDeleteDTO);

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Delete(productId);

//        // Assert
//        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
//        var product = Assert.IsType<ProductDeleteDTO>(okObjectResult.Value);
//        Assert.Equal(productId, product.ProductId);
//    }

//    [Fact]
//    public async Task Delete_ReturnsNotFound_WhenServiceReturnsNull()
//    {
//        // Arrange
//        var productId = 1;
//        var mockService = new Mock<IProductService>();
//        mockService.Setup(service => service.DeleteProduct(productId)).ReturnsAsync((ProductDeleteDTO)null);

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Delete(productId);

//        // Assert
//        Assert.IsType<NotFoundResult>(result.Result);
//    }

//    [Fact]
//    public async Task Delete_ReturnsNotFound_WhenProductDoesNotExist()
//    {
//        // Arrange
//        var productId = 1;
//        var mockService = new Mock<IProductService>();
//        mockService.Setup(service => service.DeleteProduct(productId)).ReturnsAsync((ProductDeleteDTO)null);

//        var controller = new ProductController(mockService.Object);

//        // Act
//        var result = await controller.Delete(productId);

//        // Assert
//        Assert.IsType<NotFoundResult>(result.Result);
//    }
//}