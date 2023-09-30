using EcommerceAPI.DTO.Product;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceAPI.Data
{
    public interface IProductService
    {
        Task<ProductAddDTO> AddProduct(ProductAddDTO product);
        Task<ProductDeleteDTO> DeleteProduct(int id);
        Task<ProductGetDTO> GetProduct(int id);
        Task<IEnumerable<ProductGetDTO>> GetProducts();
        Task<ProductPutDTO> UpdateProduct(int id, ProductPutDTO product);
    }
}