using EcommerceAPI.DTO.Product;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceAPI.Data
{
    public interface IProductService
    {
        Task<Product> AddProduct(Product product);
        Task<Product> DeleteProduct(int id);
        Task<Product> GetProduct(int id);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> UpdateProduct(Product product);
    }
}