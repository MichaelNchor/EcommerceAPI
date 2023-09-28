using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Models;
using EcommerceAPI.DTO.Product;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EcommerceAPIContext _context;

        public ProductController(EcommerceAPIContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductGetDTO>>> GetProducts()
        {
            var products = await _context.Product.ToListAsync();

            var result = products.Select(p => new ProductGetDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                CreatedOn = p.CreatedOn,
                UpdatedOn = p.UpdatedOn
            });

            return Ok(result);
        }

        // GET: api/Product/5
        [HttpGet("GetProduct/{id}")]
        public async Task<ActionResult<ProductGetDTO>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
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

        // PUT: api/Product/5
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductPutDTO product)
        {
            var p = new Product()
            {
                ProductId = id,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                CreatedOn = product.CreatedOn,
                UpdatedOn = DateTime.UtcNow,
            };

            if (id != p.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(p).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Product
        [HttpPost("AddProduct")]
        public async Task<ActionResult<ProductAddDTO>> PostProduct(ProductAddDTO product)
        {
            var p = new Product()
            {
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };

            _context.Product.Add(p);

            await _context.SaveChangesAsync();

            var result = new ProductAddDTO()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                CreatedOn = DateTime.UtcNow,
            };

            return CreatedAtAction("GetProduct", new { id = result.ProductId }, result);
        }

        // DELETE: api/Product/5
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult<ProductDeleteDTO>> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);

            await _context.SaveChangesAsync();

            var result = new ProductDeleteDTO()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                DeletedOn = DateTime.UtcNow,
            };

            return result;
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
