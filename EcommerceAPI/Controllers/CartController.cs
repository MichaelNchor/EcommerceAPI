using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Models;
using EcommerceAPI.DTO.Cart;
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
        public async Task<ActionResult<IEnumerable<CartGetDTO>>> GetCartItem()
        {
            var carts = await _context.Cart.Include(c => c.Product).ToListAsync();

            //Reshape response
            var result = carts.Select(p => new CartGetDTO()
            {
                CartId = p.CartId,
                ProductId = p.Product.FirstOrDefault().ProductId,
                ProductName = p.Product.FirstOrDefault().ProductName,
                UnitPrice = p.Product.FirstOrDefault().UnitPrice,
                Quantity = p.Quantity,
                AddedOn = p.AddedOn,
                UpdatedOn = p.UpdatedOn,
            });

            return Ok(result);
        }

        // GET: api/User/5
        [HttpGet("GetCartItemByID/{id}")]
        public ActionResult<CartGetDTO> GetCartItem(int id)
        {
            var cart = _context.Cart.Include(c => c.Product).FirstOrDefault(c => c.Product.Any(p => p.CartId == id));

            if (cart == null)
            {
                return NotFound("Cart Item doesn't exist!");
            }

            //Reshape response
            var result = new CartGetDTO()
            {
                CartId = cart.CartId,
                ProductId = cart.Product.FirstOrDefault().ProductId,
                ProductName = cart.Product.FirstOrDefault().ProductName,
                UnitPrice  = cart.Product.FirstOrDefault().UnitPrice,
                Quantity = cart.Quantity,
                AddedOn = cart.AddedOn,
                UpdatedOn = cart.UpdatedOn,
            };

            return result;
        }

        // POST: api/User
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("AddItemToCart")]
        public async Task<ActionResult<CartAddDTO>> AddToCartItem(int productID, int quantity = 1)
        {
            if (!ProductExists(productID))
            {
                return NotFound("Product doesn't exist! Add another.");
            }

            Cart cart; CartAddDTO result;

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

            var c = _context.Cart.Include(c => c.Product).FirstOrDefault(c => c.Product.Any(p => p.CartId == productID));

            //Reshape response
            result = new CartAddDTO()
            {
                CartId = cart.CartId,
                ProductId = c.Product.FirstOrDefault().ProductId,
                ProductName = c.Product.FirstOrDefault().ProductName,
                UnitPrice = c.Product.FirstOrDefault().UnitPrice,
                Quantity = cart.Quantity,
                AddedOn = cart.AddedOn,
                UpdatedOn = cart.UpdatedOn,
            };

            return CreatedAtAction("GetCartItem", new { id = result.CartId }, result);
        }

        // DELETE: api/User/5
        [HttpDelete("RemoveItemToCart/{id}")]
        public async Task<ActionResult<CartDeleteDTO>> DeleteCartItem(int id)
        {
            var cart = await _context.Cart.FindAsync(id);

            if (cart == null)
            {
                return NotFound("Cart Item doesn't exist!");
            }

            _context.Cart.Remove(cart);

            await _context.SaveChangesAsync();

            var c = _context.Cart.Include(c => c.Product).FirstOrDefault(c => c.Product.Any(p => p.CartId == id));

            //Reshape response
            var result = new CartDeleteDTO()
            {
                CartId = cart.CartId,
                ProductId = c.Product.FirstOrDefault().ProductId,
                ProductName = c.Product.FirstOrDefault().ProductName,
                UnitPrice = c.Product.FirstOrDefault().UnitPrice,
                Quantity = cart.Quantity,
                AddedOn = cart.AddedOn,
                DeletedOn = DateTime.UtcNow,
            };

            return result;
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
