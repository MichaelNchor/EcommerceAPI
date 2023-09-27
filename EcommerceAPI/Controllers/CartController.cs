using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Models;
using EcommerceAPI.DTO.Cart;

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
        [HttpGet("GetAllCartItems")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCartItem()
        {
            return await _context.Cart.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("GetCartItemByID/{id}")]
        public async Task<ActionResult<Cart>> GetCartItem(int id)
        {
            Cart cart = await _context.Cart.FindAsync(id);

            if (cart == null)
            {
                return NotFound("Cart Item doesn't exist!");
            }

            return cart;
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

            Cart cart;

            try
            {
                bool hasProduct = _context.Cart.Any(e => e.ProductId == productID);

                if (hasProduct)
                {
                    cart = await _context.Cart.FirstOrDefaultAsync(c => c.ProductId == productID);

                    cart.Quantity += quantity;
                    cart.UpdatedOn = DateTime.UtcNow;

                    _context.Entry(cart).State = EntityState.Modified;
                }
                else
                {
                    Product product = await _context.Product.FirstOrDefaultAsync(c => c.ProductId == productID);

                    cart = new Cart()
                    {
                        ProductId = productID,
                        Quantity = quantity,
                        AddedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Product = product,
                    };

                    _context.Cart.Add(cart);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            var result = new CartAddDTO()
            {
                CartId = cart.CartId,
                ProductId = cart.ProductId,
                ProductName = cart.Product?.ProductName,
                UnitPrice = cart.Product?.UnitPrice,
                AddedOn = cart.AddedOn,
                UpdatedOn = cart.UpdatedOn,
            };

            await _context.SaveChangesAsync();

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

            var result = new CartDeleteDTO()
            {
                CartId = cart.CartId,
                ProductId = cart.ProductId,
                ProductName = cart.Product?.ProductName,
                Quantity = cart.Quantity,
                UnitPrice = cart.Product?.UnitPrice,
                AddedOn = cart.AddedOn,
                UpdatedOn = cart.UpdatedOn,
            };

            await _context.SaveChangesAsync();

            return result;
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
