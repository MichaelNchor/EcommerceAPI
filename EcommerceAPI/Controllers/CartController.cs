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
using EcommerceAPI.Data;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        // GET: api/Cart
        [HttpGet("GetCartItems")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            var response = await _service.GetCarts();

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // GET: api/carts/products
        [HttpGet("GetCartItemsQueryable")]
        public async Task<IActionResult> GetCartProductsAsync([FromQuery] string searchValue, decimal? minPrice, decimal? maxPrice)
        {

            var response = await _service.GetCartsQueryable(searchValue, minPrice, maxPrice);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // GET: api/Cart/5
        [HttpGet("GetCartItem/{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {

            var response = await _service.GetCartById(id);

            if (response == null)
            {
                return NotFound("Cart not found!");
            }

            return Ok(response);
        }

        // POST: api/Cart
        [HttpPost("AddItemToCart")]
        public async Task<ActionResult<Cart>> AddToCart(int productID, int quantity = 1)
        {
            bool Productexists = _service.ProductExists(productID);

            if (!Productexists)
            {
                return NotFound("Product doesn't exist! Add another.");
            }

            try
            {
                bool hasProduct = _service.CartExistsWithProduct(productID);

                dynamic response;

                if (hasProduct)
                {
                    response = await _service.UpdateExistingCart(productID, quantity);
                }
                else
                {
                    response = await _service.AddToNewCart(productID, quantity); 
                }

                return CreatedAtAction("GetCart", new { id = response.CartId }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Cart/5
        [HttpDelete("RemoveItemToCart/{id}")]
        public async Task<ActionResult<Cart>> DeleteCart(int id)
        {
            var response = await _service.DeleteCart(id);

            if(response == null)
            {
                return NotFound("Cart not found!");
            }

            return Ok(response);
        }
    }
}
