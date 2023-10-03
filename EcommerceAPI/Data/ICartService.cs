using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceAPI.Data
{
    public interface ICartService
    {
        Task<Cart> AddToNewCart(int productID, int quantity = 1);
        bool CartExistsWithProduct(int id);
        Task<Cart> DeleteCart(int id);
        Task<Cart> GetCartById(int id);
        Task<IEnumerable<Cart>> GetCarts();
        Task<IEnumerable<Cart>> GetCartsQueryable([FromQuery] string searchValue, decimal? min, decimal? max);
        bool ProductExists(int id);
        Task<Cart> UpdateExistingCart(int productID, int quantity = 1);
    }
}