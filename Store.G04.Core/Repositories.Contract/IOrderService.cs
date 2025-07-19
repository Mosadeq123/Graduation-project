using Store.G04.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Core.Repositories.Contract
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail,string basketId,int deliveryMethodId, Address shippingAddress);
        Task<IEnumerable<Order>?> GetOrderForSpecificUserAsync(string buyerEmail);
        Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail,int orderId);
    }
}
