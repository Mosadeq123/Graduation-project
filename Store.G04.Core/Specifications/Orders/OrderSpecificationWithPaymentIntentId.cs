using Store.G04.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Specifications.Orders
{
    public class OrderSpecificationWithPaymentIntentId : BaseSpecifications<Order,int>
    {
        public OrderSpecificationWithPaymentIntentId(string PaymentIntentId) :base(O => O.PaymentIntentId == PaymentIntentId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
