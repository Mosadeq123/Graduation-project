﻿namespace Store.G04.Core.Entities.Order
{
    public class Order : BaseEntity<int>
    {
        public Order()
        {

        }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; }
        public int DeliveryMethodId { get; set; } // FK
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public decimal SubTotal { get; set; }
        public string PaymentIntentId { get; set; }

        // المنشئ المحدث
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        // حساب التكلفة الكلية
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
    }
}