using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceAPI.Data
{
    public interface ICartService
    {
        Task<dynamic> AddToNewCart(int productID, int quantity = 1);
        bool CartExistsWithProduct(int id);
        Task<dynamic> DeleteCart(int id);
        Task<dynamic> GetCartById(int id);
        Task<IEnumerable<dynamic>> GetCarts();
        Task<IEnumerable<dynamic>> GetCartsQueryable([FromQuery] string searchValue, decimal? min, decimal? max);
        bool ProductExists(int id);
        Task<dynamic> UpdateExistingCart(int productID, int quantity = 1);
    }
}