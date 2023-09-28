using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Models;
using EcommerceAPI.DTO.Product;
using Microsoft.CodeAnalysis;
using System.Net;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly EcommerceAPIContext _context;

        public CartController(EcommerceAPIContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        [HttpGet("GetCartItems")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCartItem()
        {
            var carts = await _context.Cart.Include(c => c.Product).ToListAsync();

            //Reshape response
            var result = carts.Select(cart => new
            {
                CartId = cart.CartId,
                Quantity = cart.Quantity,
                AddedOn = cart.AddedOn,
                UpdatedOn = cart.UpdatedOn,
                Products = cart.Product.Select(product => new
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    CreatedOn = product.CreatedOn,
                    UpdatedOn = product.UpdatedOn,
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // GET: api/carts/products
        [HttpGet("GetCartItemsQueryable")]
        public async Task<IActionResult> GetCartProductsAsync([FromQuery] string searchValue, decimal? min = 0, decimal? max = 10000000000)
        {
            var query = _context.Cart.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                // Filter carts based on the searchValue (e.g., quantity, product name, etc.)
                query = query.Where(cart =>
                    cart.Quantity.ToString().Contains(searchValue) ||
                    cart.Product.Any(product => product.ProductName.Contains(searchValue) || 
                    product.UnitPrice.ToString().Contains(searchValue) &&
                    product.UnitPrice >= min && product.UnitPrice <= max)
                );
            }

            var carts = await query.Include(cart => cart.Product).ToListAsync();

            // Reshape response
            var result = carts.Select(cart => new
            {
                CartId = cart.CartId,
                Quantity = cart.Quantity,
                AddedOn = cart.AddedOn,
                UpdatedOn = cart.UpdatedOn,
                Products = cart.Product.Select(product => new
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    CreatedOn = product.CreatedOn,
                    UpdatedOn = product.UpdatedOn,
                }).ToList()
            });

            return Ok(result);
        }

        // GET: api/Cart/5
        [HttpGet("GetCartItem/{id}")]
        public async Task<ActionResult<Cart>> GetCartItem(int id)
        {
            var cart = await _context.Cart.Include(c => c.Product).FirstOrDefaultAsync(c => c.Product.Any(p => p.CartId == id));

            if (cart == null)
            {
                return NotFound("Cart Item doesn't exist!");
            }

            //Reshape response
            var result = new
            {
                CartId = cart.CartId,
                Quantity = cart.Quantity,
                AddedOn = cart.AddedOn,
                UpdatedOn = cart.UpdatedOn,
                Products = cart.Product.Select(product => new
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    CreatedOn = product.CreatedOn,
                    UpdatedOn = product.UpdatedOn,
                }).ToList()
            };

            return Ok(result);
        }

        // POST: api/Cart
        [HttpPost("AddItemToCart")]
        public async Task<ActionResult<Cart>> AddToCartItem(int productID, int quantity = 1)
        {
            if (!ProductExists(productID))
            {
                return NotFound("Product doesn't exist! Add another.");
            }

            Cart cart;

            try
            {
                bool hasProduct = CartExistsWithProduct(productID);

                if (hasProduct)
                {
                    cart = _context.Cart.Include(c => c.Product).FirstOrDefault(c => c.Product.Any(p => p.ProductId == productID));

                    cart.Quantity += quantity;
                    cart.UpdatedOn = DateTime.UtcNow;

                    _context.Entry(cart).State = EntityState.Modified;
                }
                else
                {
                    var product = await _context.Product.FirstOrDefaultAsync(c => c.ProductId == productID);

                    cart = new Cart()
                    {
                        Quantity = quantity,
                        AddedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                    };

                    cart.Product.Add(product);

                    _context.Cart.Add(cart);
                }

                await _context.SaveChangesAsync();

                //Reshape response
                var result = new
                {
                    CartId = cart.CartId,
                    Quantity = cart.Quantity,
                    AddedOn = cart.AddedOn,
                    UpdatedOn = cart.UpdatedOn,
                    Products = cart.Product.Select(product => new
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        UnitPrice = product.UnitPrice,
                        CreatedOn = product.CreatedOn,
                        UpdatedOn = product.UpdatedOn,
                    }).ToList()
                };

                return CreatedAtAction("GetCartItem", new { id = result.CartId }, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        // DELETE: api/Cart/5
        [HttpDelete("RemoveItemToCart/{id}")]
        public async Task<ActionResult<Cart>> DeleteCartItem(int id)
        {
            var cart = await _context.Cart.FindAsync(id);

            if (cart == null)
            {
                return NotFound("Cart Item doesn't exist!");
            }

            // Find all related products and update their CartId to null
            var relatedProducts = _context.Product.Where(p => p.CartId == id).ToList();

            foreach (var product in relatedProducts)
            {
                product.CartId = null;
            }

            _context.Cart.Remove(cart);

            await _context.SaveChangesAsync();

            var result = new
            {
                CartId = cart.CartId,
                Quantity = cart.Quantity,
                AddedOn = cart.AddedOn,
                UpdatedOn = cart.UpdatedOn,
                Products = cart.Product.Select(product => new
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice,
                    CreatedOn = product.CreatedOn,
                    UpdatedOn = product.UpdatedOn,
                }).ToList()
            };

            return Ok(result);
        }

        private bool CartExistsWithProduct(int id)
        {
            return _context.Cart.Include(c => c.Product).Any(p => p.Product.Any(p => p.ProductId == id));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
