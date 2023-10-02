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

        public async Task<IEnumerable<ProductGetDTO>> GetProducts()
        {
            var products = await _dbcontext.Product.ToListAsync();

            var result = products.Select(p => new ProductGetDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                CreatedOn = p.CreatedOn,
                UpdatedOn = p.UpdatedOn
            });

            return result;
        }

        public async Task<ProductGetDTO> GetProduct(int id)
        {
            var product = await _dbcontext.Product.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            var result = new ProductGetDTO()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = product.UpdatedOn,
            };

            return result;
        }

        public async Task<ProductPutDTO> UpdateProduct(int id, ProductPutDTO product)
        {
            var p = new Product()
            {
                ProductId = id,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                CreatedOn = product.CreatedOn,
                UpdatedOn = DateTime.UtcNow,
            };

            try
            {
                _dbcontext.Entry(p).State = EntityState.Modified;

                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            var result = new ProductPutDTO()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                CreatedOn = p.CreatedOn,
                UpdatedOn = p.UpdatedOn,
            };

            return result;
        }

        public async Task<ProductAddDTO> AddProduct(ProductAddDTO product)
        {
            var p = new Product()
            {
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };

            _dbcontext.Product.Add(p);

            await _dbcontext.SaveChangesAsync();

            var result = new ProductAddDTO()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                CreatedOn = DateTime.UtcNow,
            };

            return result;
        }

        public async Task<ProductDeleteDTO> DeleteProduct(int id)
        {
            var product = await _dbcontext.Product.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            _dbcontext.Product.Remove(product);

            await _dbcontext.SaveChangesAsync();

            var result = new ProductDeleteDTO()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                DeletedOn = DateTime.UtcNow,
            };

            return result;
        }

        public bool ProductExists(int id)
        {
            return _dbcontext.Product.Any(e => e.ProductId == id);
        }
    }
}
