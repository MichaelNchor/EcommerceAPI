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
using AutoMapper;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/Product
        [HttpGet("GetProducts")]
        public async Task<ActionResult> Gets()
        {
            var response = await _service.GetProducts();

            if (response == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<ProductGetDTO>>(response));
        }

        // GET: api/Product/5
        [HttpGet("GetProduct/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var response = await _service.GetProduct(id);

            if(response == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductGetDTO>(response));
        }

        // PUT: api/Product/5
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> Put(int id, ProductPutDTO product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var response = await _service.UpdateProduct(_mapper.Map<Product>(product));

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response);
            }

            return BadRequest();
        }

        // POST: api/Product
        [HttpPost("AddProduct")]
        public async Task<ActionResult> Post(ProductAddDTO product)
        {
            if (ModelState.IsValid)
            {
                var prod = _mapper.Map<Product>(product);

                var response = await _service.AddProduct(prod);

                if(response == null)
                {
                    return BadRequest();
                }

                return Ok(_mapper.Map<ProductAddDTO>(response));
            }

            return BadRequest();
        }

        // DELETE: api/Product/5
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult> Delete(int id)
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
