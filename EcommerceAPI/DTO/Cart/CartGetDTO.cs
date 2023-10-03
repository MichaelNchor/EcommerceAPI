using EcommerceAPI.DTO.Product;
using System;
using System.Collections.Generic;

namespace EcommerceAPI.DTO.Cart
{
    public class CartGetDTO
    {
        public int CartId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<ProductGetDTO> Products { get; set; }
    }
}
