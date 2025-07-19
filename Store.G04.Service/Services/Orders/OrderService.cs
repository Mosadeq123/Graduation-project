using Store.G04.Core;
using Store.G04.Core.Entities.Order;
using Store.G04.Core.Repositories.Contract;
using Store.G04.Core.Services.Contract;
using Store.G04.Core.Specifications.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork, IWishlistRepository wishlistRepository, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _wishlistRepository = wishlistRepository;
            _paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // الحصول على السلة بناءً على المعرف
            var basket = await _wishlistRepository.GetWishlistAsync(basketId);
            if (basket is null) return null;

            // إنشاء قائمة عناصر الطلب
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                // الحصول على المنتج بناءً على المعرف
                var product = await _unitOfWork.Repository<RawMaterial, int>().GetAsync(item.Id);

                // إنشاء كائن ProductItemOrder
                var ProductOrderedItem = new ProductItemOrder(product.Id, product.NameMaterial, product.PictureUrl);

                // إنشاء عنصر الطلب
                var orderItem = new OrderItem(ProductOrderedItem, item.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            // الحصول على طريقة التوصيل
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(deliveryMethodId);

            // حساب المجموع الفرعي
            var subTotal = orderItems.Sum(i => i.Price * i.Quantity);

            // إنشاء كائن الطلب 
            if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var spec = new OrderSpecificationWithPaymentIntentId(basket.PaymentIntentId);
                var ExOrder = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
                await _unitOfWork.Repository<Order, int>().DeleteAsync(ExOrder); // تم إضافة await
            }

            var basketDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, basketDto.PaymentIntentId);

            // إضافة الطلب إلى قاعدة البيانات
            await _unitOfWork.Repository<Order, int>().AddAsync(order);

            // حفظ التغييرات
            var result = await _unitOfWork.CompleteAsysnc();
            if (result <= 0) return null;

            return order;
        }

        public async Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var Spec = new OrderSpecifications(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(Spec);
            if (order is null) return null;
            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrderForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);
            if (orders is null) return null;
            return orders;
        }
    }
}
