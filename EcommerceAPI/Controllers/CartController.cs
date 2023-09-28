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

        // GET: api/User
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

        // GET: api/User/5
        [HttpGet("GetCartItemByID/{id}")]
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

        // POST: api/User
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
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

        // DELETE: api/User/5
        [HttpDelete("RemoveItemToCart/{id}")]
        public async Task<ActionResult<Cart>> DeleteCartItem(int id)
        {
            var cart = await _context.Cart.FindAsync(id);

            if (cart == null)
            {
                return NotFound("Cart Item doesn't exist!");
            }

            // Check if there are related products associated with this cart
            var relatedProducts = _context.Product.Where(p => p.CartId == id).ToList();

            if (relatedProducts.Count > 0)
            {
                _context.Product.RemoveRange(relatedProducts);
            }

            _context.Cart.Remove(cart);

            await _context.SaveChangesAsync();

            return NoContent();
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
