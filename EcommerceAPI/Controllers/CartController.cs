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
using AutoMapper;
using EcommerceAPI.DTO.Cart;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        private readonly IMapper _mapper;

        public CartController(ICartService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/Cart
        [HttpGet("GetCarts")]
        public async Task<ActionResult> GetCarts()
        {
            var response = await _service.GetCarts();

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response.Select(cart => _mapper.Map<CartGetDTO>(cart)).ToList());
        }

        // GET: api/carts/products
        [HttpGet("GetCartsQueryable")]
        public async Task<ActionResult> GetCartsQuery([FromQuery] string searchValue, decimal? minPrice, decimal? maxPrice)
        {

            var response = await _service.GetCartsQueryable(searchValue, minPrice, maxPrice);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response.Select(cart => _mapper.Map<CartGetDTO>(cart)).ToList());
        }

        // GET: api/Cart/5
        [HttpGet("GetCart/{id}")]
        public async Task<ActionResult> GetCart(int id)
        {

            var response = await _service.GetCartById(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CartGetDTO>(response));
        }

        // POST: api/Cart
        [HttpPost("AddToCart")]
        public async Task<ActionResult> AddToCart(int productID, int quantity = 1)
        {
            bool Productexists = _service.ProductExists(productID);

            if (!Productexists)
            {
                return NotFound();
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

                return Ok(_mapper.Map<CartGetDTO>(response));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Cart/5
        [HttpDelete("RemoveToCart/{id}")]
        public async Task<ActionResult<Cart>> DeleteCart(int id)
        {
            var response = await _service.DeleteCart(id);

            if(response == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CartGetDTO>(response));
        }
    }
}
