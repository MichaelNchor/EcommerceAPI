using EcommerceAPI.DTO.Product;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Data
{
    public class CartService : ICartService
    {
        private readonly EcommerceAPIContext _dbcontext;

        public CartService(EcommerceAPIContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Cart>> GetCarts()
        {
            return await _dbcontext.Cart.Include(c => c.Product).ToListAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartsQueryable([FromQuery] string searchValue, decimal? min, decimal? max)
        {
            var query = _dbcontext.Cart.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                // Filter carts based on the searchValue (e.g., quantity, product name, etc.)
                query = query.Where(cart =>
                cart.Quantity.ToString().Contains(searchValue) ||
                cart.Product.Any(product => product.ProductName.Contains(searchValue) ||
                product.UnitPrice.ToString().Contains(searchValue) &&
                product.UnitPrice >= min && product.UnitPrice <= max));
            }

            if(min >= 0 && max >= 0 && max >= min)
            {
                query = query.Where(cart =>
                cart.Product.Any(product => product.UnitPrice >= min && product.UnitPrice <= max));
            }

            var carts = await query.Include(cart => cart.Product).ToListAsync();

            return carts;
        }

        public async Task<Cart> GetCartById(int id)
        {
            var cart = await _dbcontext.Cart.Include(c => c.Product).FirstOrDefaultAsync(c => c.Product.Any(p => p.CartId == id));

            if (cart == null)
            {
                return null;
            }

            return cart;
        }

        public async Task<Cart> AddToNewCart(int productID, int quantity = 1)
        {
            var product = await _dbcontext.Product.FirstOrDefaultAsync(c => c.ProductId == productID);

            var cart = new Cart()
            {
                Quantity = quantity,
                AddedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };

            cart.Product.Add(product);

            _dbcontext.Cart.Add(cart);

            await _dbcontext.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> UpdateExistingCart(int productID, int quantity = 1)
        {
            var cart = _dbcontext.Cart.Include(c => c.Product).FirstOrDefault(c => c.Product.Any(p => p.ProductId == productID));

            cart.Quantity += quantity;
            cart.UpdatedOn = DateTime.UtcNow;

            _dbcontext.Entry(cart).State = EntityState.Modified;

            await _dbcontext.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> DeleteCart(int id)
        {
            var cart = await _dbcontext.Cart.FindAsync(id);

            if (cart == null)
            {
                return null;
            }

            // Find all related products and update their CartId to null
            var relatedProducts = _dbcontext.Product.Where(p => p.CartId == id).ToList();

            foreach (var product in relatedProducts)
            {
                product.CartId = null;
            }

            _dbcontext.Cart.Remove(cart);

            await _dbcontext.SaveChangesAsync();

            return cart;
        }

        public bool CartExistsWithProduct(int id)
        {
            return _dbcontext.Cart.Include(c => c.Product).Any(p => p.Product.Any(p => p.ProductId == id));
        }

        public bool ProductExists(int id)
        {
            return _dbcontext.Product.Any(e => e.ProductId == id);
        }
    }
}
