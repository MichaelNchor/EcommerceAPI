using System;

namespace EcommerceAPI.DTO.Product
{
    public class ProductAddDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
