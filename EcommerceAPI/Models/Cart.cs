using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EcommerceAPI.Models
{
    public partial class Cart
    {
        public Cart()
        {
            Product = new HashSet<Product>();
        }

        public int CartId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
