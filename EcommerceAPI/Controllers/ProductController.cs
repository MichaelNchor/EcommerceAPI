using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Models;
using EcommerceAPI.DTO.Product;
using EcommerceAPI.Data;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        // GET: api/Product
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductGetDTO>>> Gets()
        {
            var response = await _service.GetProducts();

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // GET: api/Product/5
        [HttpGet("GetProduct/{id}")]
        public async Task<ActionResult<ProductGetDTO>> Get(int id)
        {
            var response = await _service.GetProduct(id);

            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // PUT: api/Product/5
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> Put(int id, ProductPutDTO product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            var response = await _service.UpdateProduct(id, product);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // POST: api/Product
        [HttpPost("AddProduct")]
        public async Task<ActionResult<ProductAddDTO>> Post(ProductAddDTO product)
        {
            if (ModelState.IsValid)
            {
                var response = await _service.AddProduct(product);

                return CreatedAtAction("GetProduct", new { id = response.ProductId }, response);
            }

            return BadRequest();
        }

        // DELETE: api/Product/5
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult<ProductDeleteDTO>> Delete(int id)
        {
            var response = await _service.DeleteProduct(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
