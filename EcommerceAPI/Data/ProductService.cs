using EcommerceAPI.DTO.Product;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Data
{
    public class ProductService : IProductService
    {
        private readonly EcommerceAPIContext _dbcontext;

        public ProductService(EcommerceAPIContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _dbcontext.Product.ToListAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _dbcontext.Product.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            product.UpdatedOn = DateTime.UtcNow; 

            _dbcontext.Entry(product).State = EntityState.Modified;

            await _dbcontext.SaveChangesAsync();

            return product;
        }

        public async Task<Product> AddProduct(Product product)
        {
            product.CreatedOn = DateTime.UtcNow;

            product.UpdatedOn = DateTime.UtcNow;

            _dbcontext.Product.Add(product);

            await _dbcontext.SaveChangesAsync();

            return product;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product = await _dbcontext.Product.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            _dbcontext.Product.Remove(product);

            await _dbcontext.SaveChangesAsync();

            return product;
        }

        public bool ProductExists(int id)
        {
            return _dbcontext.Product.Any(e => e.ProductId == id);
        }
    }
}
