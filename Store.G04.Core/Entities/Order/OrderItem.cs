using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Entities.Order
{
    public class OrderItem : BaseEntity<int>
    {
        public OrderItem()
        {

        }
        public OrderItem(ProductItemOrder rawMaterial, decimal price, int quantity)
        {
            Product = rawMaterial;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrder Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}