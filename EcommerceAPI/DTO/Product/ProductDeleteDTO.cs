using System;

namespace EcommerceAPI.DTO.Product
{
    public class ProductDeleteDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
